using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using Cdh.Toolkit.Extensions.Collections;
using Cdh.Toolkit.Extensions.Events;
using Cdh.Toolkit.Extensions.ReaderWriterLockSlim;
using Cdh.Toolkit.Collections;

namespace Cdh.Toolkit.CommandService
{
    public class Service : ICommandContext
    {
        private static readonly IList<string> emptyArgs = new string[0];

        private IDictionary<string, ICommand> CommandMap;
        private IDictionary<string, IConsoleWriter> ConsoleWriterMap;

        private ReaderWriterLockSlim CommandMapLock = new ReaderWriterLockSlim();
        private ReaderWriterLockSlim ConsoleWriterMapLock = new ReaderWriterLockSlim();

        public event EventHandler<LineWrittenEventArgs> ConsoleLineWritten;
        public event EventHandler TerminatedByUser;

        public IConsoleWriter NormalWriter { get; private set; }
        public IConsoleWriter ErrorWriter { get; private set; }

        private ICommandArgumentParser commandArgumentParser;
        protected ICommandArgumentParser CommandArgumentParser
        {
            get { return commandArgumentParser; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                commandArgumentParser = value;
            }
        }

        public ICollection<ICommand> Commands { get; private set; }

        protected internal ICollection<IConsoleWriter> ConsoleWriters { get; private set; }

        public Service()
        {
            CommandArgumentParser = DefaultCommandArgumentParser.Instance;

            CommandMap = new Dictionary<string, ICommand>();
            ConsoleWriterMap = new Dictionary<string, IConsoleWriter>();

            Commands = new ReadOnlyCollection<ICommand>(
                new SynchronizedCollection<ICommand>(CommandMap.Values, EnumerateBehavior.Lock, CommandMapLock));

            ConsoleWriters = new ReadOnlyCollection<IConsoleWriter>(
                new SynchronizedCollection<IConsoleWriter>(ConsoleWriterMap.Values, EnumerateBehavior.Lock, ConsoleWriterMapLock));

            NormalWriter = new BasicConsoleWriter("normal");
            ErrorWriter = new BasicConsoleWriter("error");

            RegisterConsoleWriter(NormalWriter);
            RegisterConsoleWriter(ErrorWriter);
        }

        protected void RegisterCommand(ICommand command)
        {
            if (command == null)
                throw new ArgumentNullException("command");

            using (CommandMapLock.Write())
                CommandMap[command.Name.ToLower()] = command;
        }

        protected void RegisterConsoleWriter(IConsoleWriter writer)
        {
            if (writer == null)
                throw new ArgumentNullException("writer");

            writer.LineWritten += ProxyLineWritten;

            IConsoleWriter oldWriter;

            using (ConsoleWriterMapLock.UpgradeableRead())
            {
                if (ConsoleWriterMap.ContainsKey(writer.Name) && (writer.Name == "normal" || writer.Name == "error"))
                    throw new ArgumentException("Cannot replace the normal or error console writers.", "writer");

                using (ConsoleWriterMapLock.Write())
                {
                    oldWriter = ConsoleWriterMap.GetOrDefault(writer.Name);
                    ConsoleWriterMap[writer.Name] = writer;
                }
            }

            if (oldWriter != null)
                oldWriter.LineWritten -= ProxyLineWritten;
        }

        protected bool UnregisterCommand(string name)
        {
            using (CommandMapLock.Write())
                return CommandMap.Remove(name);
        }

        protected bool UnregisterConsoleWriter(string name)
        {
            if (name == "normal" || name == "error")
                return false;

            IConsoleWriter oldWriter;
            using (ConsoleWriterMapLock.UpgradeableRead())
            {
                if (!ConsoleWriterMap.TryGetValue(name, out oldWriter))
                    return false;

                using (ConsoleWriterMapLock.Write())
                    ConsoleWriterMap.Remove(name);
            }

            oldWriter.LineWritten -= ProxyLineWritten;
            return true;
        }

        public IConsoleWriter GetConsoleWriter(string name)
        {
            using (ConsoleWriterMapLock.Read())
                return ConsoleWriterMap.GetOrDefault(name);
        }

        private void ProxyLineWritten(object sender, LineWrittenEventArgs args)
        {
            ConsoleLineWritten.Fire(this, args);
        }

        protected virtual bool HandleException(ICommand command, ICommandContext context, Exception exception)
        {
            if (exception is CommandException || exception is ArgumentsParseException)
            {
                context.ErrorWriter.WriteLine(exception.Message);
            }
            else
            {
                if (command == null)
                    context.ErrorWriter.WriteLine("Unhandled exception while resolving command:");
                else
                    context.ErrorWriter.WriteLine("Unhandled exception while executing command " + command.Name + ":");

                context.ErrorWriter.WriteLine();
                context.ErrorWriter.WriteLine(exception.ToString());
            }

            return true;
        }

        public virtual void ExecuteCommand(string commandName)
        {
            ExecuteCommand(commandName, this);
        }

        public virtual void ExecuteCommand(string commandName, ICommandContext context)
        {
            ExecuteCommand(commandName, emptyArgs, context);
        }

        public virtual void ExecuteCommand(string commandName, IList<string> arguments)
        {
            ExecuteCommand(commandName, arguments, this);
        }

        public virtual void ExecuteCommand(string commandName, IList<string> arguments, ICommandContext context)
        {
            ICommand command;

            try
            {
                command = ResolveCommand(commandName);
            }
            catch (Exception ex)
            {
                if (!HandleException(null, context, ex))
                    throw;

                return;
            }

            ExecuteCommand(command, arguments, context);
        }

        protected virtual void ExecuteCommand(ICommand command, IList<string> arguments, ICommandContext context)
        {
            if (arguments == null)
                arguments = emptyArgs;

            try
            {
                command.Execute(context, arguments);
            }
            catch (Exception ex)
            {
                if (!HandleException(command, context, ex))
                    throw;
            }
        }

        public virtual void ExecuteCommandLine(string commandLine)
        {
            ExecuteCommandLine(commandLine, this);
        }

        public virtual void ExecuteCommandLine(string commandLine, ICommandContext context)
        {
            var parts = commandLine.Split(Constants.SpaceCharArray, 2, StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length == 0)
                return;

            string commandName = parts[0];

            ICommand command = null;
            IList<string> arguments;

            try
            {
                command = ResolveCommand(commandName);

                if (command.MaxArguments < 1 || parts.Length != 2)
                {
                    arguments = emptyArgs;
                }
                else
                {
                    ICommandArgumentParser parser = (command as ICommandArgumentParser) ?? CommandArgumentParser;

                    arguments = parser.ParseArguments(parts[1], command.MaxArguments) ?? emptyArgs;
                }
            }
            catch (Exception ex)
            {
                if (!HandleException(command, context, ex))
                    throw;

                return;
            }

            ExecuteCommand(command, arguments, context);
        }

        public virtual ICommand ResolveCommand(string commandName)
        {
            var commands = Commands.ResolveName(commandName, i => i.Name).ToList();

            if (commands.Count == 1)
                return commands[0];

            if (commands.Count == 0)
                throw new CommandNotFoundException(commandName);

            throw new AmbiguousCommandException(commands.Select(i => i.Name));
        }

        protected internal void FireTerminatedByUser()
        {
            TerminatedByUser.Fire(this);
        }

        #region ICommandContext Members

        Service ICommandContext.Service
        {
            get { return this; }
        }

        #endregion
    }
}

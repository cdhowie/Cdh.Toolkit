using System;
using System.Collections.Generic;
using System.Threading;

using Cdh.Toolkit.Extensions.Collections;
using Cdh.Toolkit.Extensions.Events;
using Cdh.Toolkit.Extensions.ReaderWriterLockSlim;

namespace Cdh.Toolkit.CommandService
{
    public class Service : ICommandArgumentParser, ICommandContext
    {
        private static readonly IList<string> emptyArgs = new string[0];
        private static readonly char[] spaceCharArray = new[] { ' ' };

        private IDictionary<string, ICommand> CommandMap;
        private IDictionary<string, IConsoleWriter> ConsoleWriterMap;

        private ReaderWriterLockSlim CommandMapLock = new ReaderWriterLockSlim();
        private ReaderWriterLockSlim ConsoleWriterMapLock = new ReaderWriterLockSlim();

        public event EventHandler<LineWrittenEventArgs> ConsoleLineWritten;
        public event EventHandler UserTerminated;

        public IConsoleWriter NormalWriter { get; private set; }
        public IConsoleWriter ErrorWriter { get; private set; }

        protected internal IEnumerable<ICommand> Commands
        {
            get
            {
                using (CommandMapLock.Read())
                    foreach (ICommand command in CommandMap.Values)
                        yield return command;
            }
        }

        protected internal IEnumerable<IConsoleWriter> ConsoleWriters
        {
            get
            {
                using (ConsoleWriterMapLock.Read())
                    foreach (IConsoleWriter writer in ConsoleWriterMap.Values)
                        yield return writer;
            }
        }

        public Service()
        {
            CommandMap = new Dictionary<string, ICommand>();
            ConsoleWriterMap = new Dictionary<string, IConsoleWriter>();

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
                CommandMap[command.Name] = command;
        }

        protected void RegisterConsoleWriter(IConsoleWriter writer)
        {
            if (writer == null)
                throw new ArgumentNullException("writer");

            writer.LineWritten += ProxyLineWritten;

            IConsoleWriter oldWriter;

            using (ConsoleWriterMapLock.UpgradeableRead())
            {
                if (ConsoleWriterMap.ContainsKey(writer.Name) && writer.Name == "normal" || writer.Name == "error")
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

        public virtual IConsoleWriter GetWriter(string name)
        {
            using (ConsoleWriterMapLock.Read())
                return ConsoleWriterMap.GetOrDefault(name);
        }

        private void ProxyLineWritten(object sender, LineWrittenEventArgs args)
        {
            ConsoleLineWritten.Fire(this, args);
        }

        protected virtual bool HandleException(ICommand command, Exception exception)
        {
            CommandException commandException = exception as CommandException;
            if (commandException != null)
            {
                ErrorWriter.WriteLine(commandException.Message);
            }
            else
            {
                ErrorWriter.WriteLine("Unhandled exception while executing command " + command.Name + ":");
                ErrorWriter.WriteLine();
                ErrorWriter.WriteLine(exception.ToString());
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

            using (CommandMapLock.Read())
                command = CommandMap.GetOrDefault(commandName);

            if (command == null)
            {
                ErrorWriter.WriteLine("No such command: " + commandName);
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
                if (!HandleException(command, ex))
                    throw;
            }
        }

        public virtual void ExecuteCommandLine(string commandLine)
        {
            ExecuteCommandLine(commandLine, this);
        }

        public virtual void ExecuteCommandLine(string commandLine, ICommandContext context)
        {
            var parts = commandLine.Split(spaceCharArray, 2, StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length == 0)
                return;

            string commandName = parts[0];

            ICommand command;
            using (CommandMapLock.Read())
                command = CommandMap.GetOrDefault(commandName);

            if (command == null)
            {
                ErrorWriter.WriteLine("No such command: " + commandName);
                return;
            }

            IList<string> arguments;

            if (command.MaxArguments < 1 || parts.Length != 2)
            {
                arguments = emptyArgs;
            }
            else
            {
                ICommandArgumentParser parser = (command as ICommandArgumentParser) ?? this;
                arguments = parser.ParseArguments(parts[1], command.MaxArguments) ?? emptyArgs;
            }

            ExecuteCommand(command, arguments, context);
        }

        protected internal void FireUserTerminated()
        {
            UserTerminated.Fire(this);
        }

        #region ICommandArgumentParser Members

        protected virtual IList<string> ParseArguments(string arguments, int maxArguments)
        {
            return arguments.Split(spaceCharArray, maxArguments, StringSplitOptions.RemoveEmptyEntries);
        }

        IList<string> ICommandArgumentParser.ParseArguments(string arguments, int maxArguments)
        {
            return ParseArguments(arguments, maxArguments);
        }

        #endregion

        #region ICommandContext Members

        Service ICommandContext.Service
        {
            get { return this; }
        }

        #endregion
    }
}

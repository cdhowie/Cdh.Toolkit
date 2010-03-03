using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cdh.Toolkit.CommandService.Tests.Commands
{
    internal class LoggingCommand : ICommand
    {
        public LoggingCommand(string name)
        {
            Name = name;
        }

        #region ICommand Members

        public string Name { get; private set; }

        public string Description
        {
            get { throw new NotImplementedException(); }
        }

        public int MaxArguments
        {
            get { return 0; }
        }

        public void Execute(ICommandContext context, IList<string> arguments)
        {
            context.GetConsoleWriter("command-log").WriteLine(Name);
        }

        #endregion
    }
}

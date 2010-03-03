using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cdh.Toolkit.CommandService.Tests.Commands;

namespace Cdh.Toolkit.CommandService.Tests
{
    internal class TestService : Service
    {
        public IConsoleWriter CommandLogWriter { get; private set; }

        public TestService()
        {
            CommandLogWriter = new BasicConsoleWriter("command-log");
            RegisterConsoleWriter(CommandLogWriter);

            RegisterCommand(new EchoCommand());

            RegisterCommand(new LoggingCommand("ambig-foo"));
            RegisterCommand(new LoggingCommand("ambig-foobar"));
            RegisterCommand(new LoggingCommand("ambig-bar"));
        }

        protected override bool HandleException(ICommand command, ICommandContext context, Exception exception)
        {
            return false;
        }
    }
}

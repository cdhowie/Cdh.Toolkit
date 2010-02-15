using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cdh.Toolkit.CommandService
{
    public class CommandNotFoundException : CommandException
    {
        public string CommandName { get; private set; }

        public CommandNotFoundException(string commandName)
            : base("No such command: " + commandName + ".")
        {
            CommandName = commandName;
        }
    }
}

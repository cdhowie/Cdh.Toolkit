using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cdh.Toolkit.CommandService
{
    public class AmbiguousCommandException : CommandException
    {
        public ICollection<string> Commands { get; private set; }

        public AmbiguousCommandException(IEnumerable<string> commands)
            : base(CreateExceptionMessage(commands))
        {
            Commands = commands.ToList().AsReadOnly();
        }

        private static string CreateExceptionMessage(IEnumerable<string> commands)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("Command is ambiguous between:");
            foreach (string i in commands.OrderBy(i => i))
            {
                sb.Append(' ');
                sb.Append(i);
            }

            sb.Append('.');

            return sb.ToString();
        }
    }
}

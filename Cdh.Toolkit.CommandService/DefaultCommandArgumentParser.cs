using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cdh.Toolkit.CommandService
{
    public sealed class DefaultCommandArgumentParser : ICommandArgumentParser
    {
        public static readonly DefaultCommandArgumentParser Instance = new DefaultCommandArgumentParser();

        private DefaultCommandArgumentParser() { }

        public IList<string> ParseArguments(string arguments, int maxArguments)
        {
            return ParseArguments(arguments, maxArguments);
        }
    }
}

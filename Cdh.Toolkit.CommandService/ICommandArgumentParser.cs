using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cdh.Toolkit.CommandService
{
    public interface ICommandArgumentParser
    {
        IList<string> ParseArguments(string arguments, int maxArguments);
    }
}

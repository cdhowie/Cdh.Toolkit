using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cdh.Toolkit.CommandService
{
    public interface ICommandContext
    {
        IConsoleWriter NormalWriter { get; }
        IConsoleWriter ErrorWriter { get; }
        Service Service { get; }

        IConsoleWriter GetConsoleWriter(string name);
    }
}

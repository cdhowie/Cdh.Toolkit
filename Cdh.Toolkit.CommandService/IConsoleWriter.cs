using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cdh.Toolkit.CommandService
{
    public interface IConsoleWriter
    {
        string Name { get; }

        event EventHandler<LineWrittenEventArgs> LineWritten;

        void WriteLine();
        void WriteLine(string line);
        void WriteLine(string format, params object[] arguments);
    }
}

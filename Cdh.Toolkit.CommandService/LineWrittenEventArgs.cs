using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cdh.Toolkit.CommandService
{
    public class LineWrittenEventArgs : EventArgs
    {
        public string Line { get; private set; }
        public IConsoleWriter ConsoleWriter { get; private set; }

        public LineWrittenEventArgs(string line, IConsoleWriter writer)
        {
            Line = line;
            ConsoleWriter = writer;
        }
    }
}

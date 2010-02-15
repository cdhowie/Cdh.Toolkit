using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Cdh.Toolkit.Extensions.Events;

namespace Cdh.Toolkit.CommandService
{
    public class BasicConsoleWriter : IConsoleWriter
    {
        private readonly LineWrittenEventArgs BlankLineEventArgs;

        public BasicConsoleWriter(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Is null or empty", "name");

            Name = name;
            BlankLineEventArgs = new LineWrittenEventArgs("", this);
        }

        #region IConsoleWriter Members

        public string Name { get; private set; }

        public event EventHandler<LineWrittenEventArgs> LineWritten;

        public void WriteLine()
        {
            LineWritten.Fire(this, BlankLineEventArgs);
        }

        public void WriteLine(string line)
        {
            LineWritten.Fire(this, new LineWrittenEventArgs(line, this));
        }

        public void WriteLine(string format, params object[] arguments)
        {
            var handler = LineWritten;
            if (handler == null)
                return;

            handler(this, new LineWrittenEventArgs(string.Format(format, arguments), this));
        }

        #endregion
    }
}

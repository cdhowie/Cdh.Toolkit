using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Cdh.Toolkit.Extensions.Events;
using System.IO;

namespace Cdh.Toolkit.CommandService
{
    public class BasicConsoleWriter : IConsoleWriter
    {
        protected readonly LineWrittenEventArgs BlankLineEventArgs;

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
            var handler = LineWritten;
            if (handler == null)
                return;

            WriteLineWithNewlines(handler, line);
        }

        public void WriteLine(string format, params object[] arguments)
        {
            var handler = LineWritten;
            if (handler == null)
                return;

            WriteLineWithNewlines(handler, string.Format(format, arguments));
        }

        protected void WriteLineWithNewlines(EventHandler<LineWrittenEventArgs> handler, string line)
        {
            if (!line.Contains('\r') && !line.Contains('\n'))
            {
                handler(this, new LineWrittenEventArgs(line, this));
                return;
            }

            foreach (var i in GetLines(line))
                handler(this, i);
        }

        private IEnumerable<LineWrittenEventArgs> GetLines(string str)
        {
            string line;

            int blanks = 0;

            using (StringReader reader = new StringReader(str))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.Trim() == "")
                    {
                        blanks++;
                    }
                    else
                    {
                        while (blanks > 0)
                        {
                            yield return BlankLineEventArgs;
                            blanks--;
                        }

                        yield return new LineWrittenEventArgs(line, this);
                    }
                }
            }
        }

        #endregion
    }
}

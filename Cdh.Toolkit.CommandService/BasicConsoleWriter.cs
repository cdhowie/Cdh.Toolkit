//
// BasicConsoleWriter.cs
//
// Author:
//       Chris Howie <me@chrishowie.com>
//
// Copyright (c) 2010 Chris Howie
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

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

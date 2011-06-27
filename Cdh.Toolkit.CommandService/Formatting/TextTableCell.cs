//
// TextTableCell.cs
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

namespace Cdh.Toolkit.CommandService.Formatting
{
    public class TextTableCell
    {
        private string contents;

        public string Contents
        {
            get { return contents; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                contents = value;
            }
        }

        public Alignment Alignment { get; set; }

        internal TextTableCell(string contents) : this(contents, Alignment.Unspecified) { }

        internal TextTableCell(string contents, Alignment alignment)
        {
            Contents = contents;
            Alignment = alignment;
        }

        internal void RenderInto(StringBuilder buffer, TextTableColumn column, int cellWidth)
        {
            int lpad, rpad;
            Alignment effective = Alignment.Or(column.Alignment).Or(Alignment.Left);

            switch (effective)
            {
                case Alignment.Left:
                    lpad = 0;
                    rpad = cellWidth - Contents.Length;
                    break;

                case Alignment.Right:
                    lpad = cellWidth - Contents.Length;
                    rpad = 0;
                    break;

                case Alignment.Center:
                    lpad = (cellWidth - Contents.Length) / 2;
                    rpad = cellWidth - lpad - Contents.Length;
                    break;

                default:
                    goto case Alignment.Left;
            }

            while (lpad-- > 0)
                buffer.Append(' ');

            buffer.Append(Contents);

            while (rpad-- > 0)
                buffer.Append(' ');
        }
    }
}

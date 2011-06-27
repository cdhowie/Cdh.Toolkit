//
// TextTableRow.cs
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
    public class TextTableRow : TextTableRowBase
    {
        private TextTableCell[] cells;

        public IList<TextTableCell> Cells { get; private set; }

        internal TextTableRow(int cells)
        {
            if (cells < 1)
                throw new ArgumentOutOfRangeException("cells", "Must be >= 1");

            this.cells = new TextTableCell[cells];
            Cells = Array.AsReadOnly(this.cells);

            for (int i = 0; i < cells; i++)
                this.cells[i] = new TextTableCell("");
        }

        internal TextTableRow(IEnumerable<TextTableCell> cells)
        {
            this.cells = cells.ToArray();
            Cells = Array.AsReadOnly(this.cells);
        }

        internal override void RenderInto(StringBuilder buffer, IList<TextTableColumn> columns, int[] cellWidths)
        {
            buffer.Append('|');

            for (int i = 0; i < cells.Length; i++)
            {
                buffer.Append(' ');
                cells[i].RenderInto(buffer, columns[i], cellWidths[i]);
                buffer.Append(" |");
            }
        }
    }
}

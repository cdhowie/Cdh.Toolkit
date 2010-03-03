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

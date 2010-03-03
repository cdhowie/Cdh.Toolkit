using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cdh.Toolkit.CommandService.Formatting
{
    internal class TextTableEnclosingRow : TextTableRowBase
    {
        public static readonly TextTableEnclosingRow Instance = new TextTableEnclosingRow();

        private TextTableEnclosingRow() { }

        internal override void RenderInto(StringBuilder buffer, IList<TextTableColumn> columns, int[] cellWidths)
        {
            buffer.Append('+');

            int totalWidth = 0;

            foreach (int width in cellWidths)
                totalWidth += width + 3;

            while (--totalWidth > 0)
                buffer.Append('-');

            buffer.Append('+');
        }
    }
}

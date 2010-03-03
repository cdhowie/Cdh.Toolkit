using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cdh.Toolkit.CommandService.Formatting
{
    public class TextTableRowSeparator : TextTableRowBase
    {
        internal static readonly TextTableRowSeparator Instance = new TextTableRowSeparator();

        private TextTableRowSeparator() { }

        internal override void RenderInto(StringBuilder buffer, IList<TextTableColumn> columns, int[] cellWidths)
        {
            buffer.Append('|');

            foreach (int width in cellWidths)
            {
                for (int i = -2; i < width; i++)
                    buffer.Append('-');

                buffer.Append('|');
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cdh.Toolkit.CommandService.Formatting
{
    public abstract class TextTableRowBase
    {
        internal abstract void RenderInto(StringBuilder buffer, IList<TextTableColumn> columns, int[] cellWidths);
    }
}

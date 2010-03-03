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

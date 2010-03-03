using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cdh.Toolkit.CommandService.Formatting
{
    public class TextTableColumn
    {
        public Alignment Alignment { get; set; }

        private string header;

        public string Header
        {
            get { return header; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                header = value;
            }
        }

        internal TextTableColumn(string header) : this(header, Alignment.Unspecified) { }

        internal TextTableColumn(string header, Alignment alignment)
        {
            Header = header;
            Alignment = alignment;
        }
    }
}

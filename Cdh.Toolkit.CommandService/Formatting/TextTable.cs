using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cdh.Toolkit.Collections;

namespace Cdh.Toolkit.CommandService.Formatting
{
    public class TextTable
    {
        private List<TextTableColumn> columns;
        private List<TextTableRowBase> rows;

        public IList<TextTableColumn> Columns { get; private set; }
        public IList<TextTableRowBase> Rows { get; private set; }

        public TextTable()
        {
            columns = new List<TextTableColumn>();
            rows = new List<TextTableRowBase>();

            Columns = new ReadOnlyList<TextTableColumn>(columns);
            Rows = new ReadOnlyList<TextTableRowBase>(rows);
        }

        private void CheckHasNoRows()
        {
            if (rows.Count != 0)
                throw new InvalidOperationException("Cannot add columns while there are rows.");
        }

        private void CheckHasColumns()
        {
            if (columns.Count == 0)
                throw new InvalidOperationException("Cannot do that until there are columns.");
        }

        public TextTableColumn AppendColumn(string header)
        {
            CheckHasNoRows();

            var column = new TextTableColumn(header);
            columns.Add(column);

            return column;
        }

        public TextTableColumn AppendColumn(string header, Alignment alignment)
        {
            CheckHasNoRows();

            var column = new TextTableColumn(header, alignment);
            columns.Add(column);

            return column;
        }

        public void AppendColumns(params string[] headers)
        {
            CheckHasNoRows();

            columns.AddRange(headers.Select(i => new TextTableColumn(i)));
        }

        public TextTableRow AppendRow()
        {
            CheckHasColumns();

            var row = new TextTableRow(columns.Count);
            rows.Add(row);

            return row;
        }

        public TextTableRow AppendRow(params string[] cells)
        {
            CheckHasColumns();

            if (cells.Length > columns.Count)
                throw new ArgumentException("cells", "Number of cells is greater than number of columns.");

            var row = new TextTableRow(columns.Count);
            rows.Add(row);

            for (int i = 0; i < cells.Length; i++)
                row.Cells[i].Contents = cells[i];

            return row;
        }

        public void AppendRowSeparator()
        {
            rows.Add(TextTableRowSeparator.Instance);
        }

        public void RenderInto(StringBuilder buffer)
        {
            CheckHasColumns();

            int[] maxWidths = new int[columns.Count];

            for (int i = 0; i < columns.Count; i++)
                maxWidths[i] = columns[i].Header.Length;

            foreach (var row in rows.OfType<TextTableRow>())
            {
                for (int i = 0; i < columns.Count; i++)
                    maxWidths[i] = Math.Max(maxWidths[i], row.Cells[i].Contents.Length);
            }

            bool newline = false;

            foreach (var i in RowPlan)
            {
                if (newline)
                    buffer.AppendLine();

                newline = true;
                i.RenderInto(buffer, columns, maxWidths);
            }
        }

        public override string ToString()
        {
            var buffer = new StringBuilder();
            RenderInto(buffer);

            return buffer.ToString();
        }

        private IEnumerable<TextTableRowBase> RowPlan
        {
            get
            {
                yield return TextTableEnclosingRow.Instance;

                yield return new TextTableRow(columns.Select(i => new TextTableCell(i.Header, i.Alignment)));

                yield return TextTableRowSeparator.Instance;

                foreach (var i in rows)
                    yield return i;

                yield return TextTableEnclosingRow.Instance;
            }
        }
    }
}

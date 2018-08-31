using System.Collections.Generic;

namespace DCode.Models.Common
{
    public class TableRow
    {
        public int Index { get; set; }

        public IList<TableColumn> Column { get; set; }

        public TableRow()
        {
            Column = new List<TableColumn>();
        }

        public TableRow(int index)
        {
            Index = index;

            Column = new List<TableColumn>();
        }
    }
}

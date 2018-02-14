using System.Collections.Generic;
using System.Linq;

namespace DCode.Models.Common
{
    public class DatabaseTable
    {
        public IList<TableRow> Table { get; set; }

        public DatabaseTable()
        {
            Table = new List<TableRow>();
        }

        public IList<string> Columns =>
            Table?.Select(x => x.Column).FirstOrDefault()?.Select(x => x.ColumnName)?.ToList();
    }
}

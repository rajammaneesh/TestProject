using DCode.Models.Common;

namespace DCode.Web.Models
{
    public class DbRecordsViewModel
    {
        public string Query { get; set; }

        public DatabaseTable Records { get; set; }
    }
}
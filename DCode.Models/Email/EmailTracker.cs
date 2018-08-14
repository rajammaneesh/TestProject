using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCode.Models.Email
{
    public class EmailTracker
    {
        public int Id { get; set; }

        public string Subject { get; set; }

        public string ToAddresses { get; set; }

        public List<string> CcAddresses { get; set; }

        public List<string> BccAddresses { get; set; }

        public DateTime SentDate { get; set; }

        public string TaskId { get; set; }

        public string Source { get; set; }

        public string Body { get; set; }
    }
}

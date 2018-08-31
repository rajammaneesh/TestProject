using System;
using System.Collections.Generic;

namespace DCode.Models.Email
{
    public class EmailTracker
    {
        public EmailTracker()
        {
            CcAddresses = new List<string>();
            BccAddresses = new List<string>();
            SentDate = DateTime.Now;
        }

        public int Id { get; set; }

        public string Subject { get; set; }

        public string ToAddresses { get; set; }

        public List<string> CcAddresses { get; set; }

        public List<string> BccAddresses { get; set; }

        public DateTime SentDate { get; set; }       

        public int? TaskId { get; set; }

        public string Source { get; set; }

        public string Body { get; set; }
    }
}

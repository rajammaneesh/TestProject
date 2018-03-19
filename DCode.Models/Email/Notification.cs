using System;
using System.Collections.Generic;

namespace DCode.Models.Email
{
    public class Notification
    {
        public List<string> CcAddresses { get; set; }

        public List<string> BccAddresses { get; set; }

        public string ToAddresses { get; set; }

        public string Skill { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace DCode.Models.Email
{
    public class ClientServiceNotificationContent : INotificationContent
    {
        public IEnumerable<Tuple<string, string>> ProjectData { get; set; }

        public string Skill { get; set; }
    }
}

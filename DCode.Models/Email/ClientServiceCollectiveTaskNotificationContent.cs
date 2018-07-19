using System;
using System.Collections.Generic;

namespace DCode.Models.Email
{
    public class ClientServiceCollectiveTaskNotificationContent : INotificationContent
    {
        public IEnumerable<Tuple<string, string,string>> ProjectData { get; set; }

        public string OfferingName { get; set; }
    }
}

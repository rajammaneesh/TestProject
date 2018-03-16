using System;
using System.Collections.Generic;

namespace DCode.Models.Email
{
    public class FirmInitiativeTaskNotificationContent : INotificationContent
    {
        public IEnumerable<Tuple<string, string, string>> ProjectData { get; set; }
    }
}

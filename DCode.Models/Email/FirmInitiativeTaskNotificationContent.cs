using System;
using System.Collections.Generic;

namespace DCode.Models.Email
{
    public class FirmInitiativeTaskNotificationContent : INotificationContent
    {
        public IEnumerable<Tuple<string, string, string, int>> ProjectData { get; set; }
    }
}

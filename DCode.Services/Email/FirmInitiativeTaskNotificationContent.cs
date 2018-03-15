using System;
using DCode.Models.Common;
using DCode.Models.Email;

namespace DCode.Services.Email
{
    public class FirmInitiativeTaskNotificationContent : ITaskNotificationContent
    {
        public string GetDynamicTableContent(INotificationContent dynamicContent)
        {
            throw new NotImplementedException();
        }

        public string GetEmailBody(INotificationContent content)
        {
            throw new NotImplementedException();
        }

        public string GetSubject(ITaskNotificationSubject subject)
        {
            throw new NotImplementedException();
        }
    }
}

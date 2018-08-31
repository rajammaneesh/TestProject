using DCode.Models.Email;
using System;
using System.Collections.Generic;

namespace DCode.Models.Common
{
    public interface ITaskNotificationContent
    {
        string GetSubject(ITaskNotificationSubject subject);

        string GetEmailBody(INotificationContent content);

        string GetDynamicTableContent(INotificationContent content);
    }
}

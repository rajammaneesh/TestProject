using DCode.Models.Email;
using System;
using System.Collections.Generic;

namespace DCode.Models.Common
{
    public interface IEmailService
    {
        void SendBulkEmail(IEnumerable<Notification> notifications);
    }
}

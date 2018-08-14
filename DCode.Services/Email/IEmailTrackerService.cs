using DCode.Models.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCode.Services.Email
{
    public interface IEmailTrackerService
    {
        void InsertEmailDetails(EmailTracker emailTracker);
    }
}

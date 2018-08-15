using DCode.Data.EmailRepository;
using DCode.Models.Email;
using DCode.Services.Base;
using DCode.Services.ModelFactory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCode.Services.Email
{
    public class EmailTrackerService : BaseService, IEmailTrackerService
    {
        private IEmailRepository _emailRepository;
        private EmailModelFactory _emailModelFactory;

        public EmailTrackerService(IEmailRepository emailRepository)
        {
            _emailRepository = emailRepository;
        }

        public int InsertEmail(EmailTracker emailTracker)
        {
            var dbEmail = _emailModelFactory.CreateModel(emailTracker);
            var result = _emailRepository.InsertEmailDetails(emailTracker);
            return result;
        }
    }
}

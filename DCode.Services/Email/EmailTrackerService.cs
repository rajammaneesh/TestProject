using DCode.Data.EmailRepository;
using DCode.Models.Email;
using DCode.Services.Base;
using DCode.Services.ModelFactory;

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
            var result = _emailRepository.InsertEmailDetails(dbEmail);
            return result;
        }
    }
}

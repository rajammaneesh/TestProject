using DCode.Models.Email;
using DCode.Services.ModelFactory.CommonFactory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCode.Services.ModelFactory
{
    public class EmailModelFactory : IModelFactory<emailtracker>
    {
        public EmailModelFactory()
        {
        }
        public TModel CreateModel<TModel>(emailtracker input) where TModel : class
        {
            var emailModel = new EmailTracker();
            emailModel.Subject = input.Subject;
            emailModel.ToAddresses = input.ToAddresses;
            emailModel.CcAddresses = input.CcAddresses;
            emailModel.BccAddresses = input.BccAddresses;
            emailModel.Body = input.Body;
            emailModel.SentDate = input.SentDate;
            emailModel.TaskId = input.TaskId;
            emailModel.Source = input.Source;
            emailModel.Id = input.Id;

            return emailModel as TModel;
        }

        public emailtracker CreateModel<TModel>(TModel input) where TModel : class
        {
            if (input.GetType() == typeof(EmailTracker))
            {
                var emailTrackerDb = new emailtracker();
                var inputModel = input as EmailTracker;
               
                return emailTrackerDb;
            }
            return null;
        }
    }
}

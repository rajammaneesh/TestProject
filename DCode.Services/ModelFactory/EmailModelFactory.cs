using DCode.Data.DbContexts;
using DCode.Models.Email;
using DCode.Services.ModelFactory.CommonFactory;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DCode.Services.ModelFactory
{
    public class EmailModelFactory : IModelFactory<email_tracker>
    {
        public EmailModelFactory()
        {
        }
        public TModel CreateModel<TModel>(email_tracker input) where TModel : class
        {
            var emailModel = new EmailTracker();
            emailModel.Subject = input.subject;
            emailModel.ToAddresses = input.to_addresses;
            emailModel.Body = input.body;
            emailModel.SentDate = input.sent_date;
            emailModel.Source = input.source;
            emailModel.Id = input.id;
            if (input.cc_addresses != null)
            {
                if (input.cc_addresses.Contains(";"))
                {
                    emailModel.CcAddresses.AddRange(input.cc_addresses.Split(';'));
                }
                else
                {
                    emailModel.CcAddresses.Add(input.cc_addresses);
                }
            }
            if (input.bcc_Addresses != null)
            {
                if (input.bcc_Addresses.Contains(";"))
                {
                    emailModel.BccAddresses.AddRange(input.bcc_Addresses.Split(';'));
                }
                else
                {
                    emailModel.BccAddresses.Add(input.bcc_Addresses);
                }
            }

            return emailModel as TModel;
        }

        public email_tracker CreateModel<TModel>(TModel input) where TModel : class
        {
            if (input.GetType() == typeof(EmailTracker))
            {
                var emailTrackerDb = new email_tracker();
                var inputModel = input as EmailTracker;
                emailTrackerDb.subject = inputModel.Subject;
                emailTrackerDb.to_addresses = inputModel.ToAddresses;
                emailTrackerDb.body = inputModel.Body;
                emailTrackerDb.sent_date = inputModel.SentDate;
                emailTrackerDb.source = inputModel.Source;
              //  emailTrackerDb.id = inputModel.Id;
                if (inputModel.CcAddresses != null)
                {
                    if(inputModel.CcAddresses.Contains(";"))
                    {
                        emailTrackerDb.cc_addresses = string.Join(";", inputModel.CcAddresses);
                    }
                    else
                    {
                        emailTrackerDb.cc_addresses = inputModel.CcAddresses.FirstOrDefault();
                    }
                }
                if (inputModel.BccAddresses != null)
                {
                    if (inputModel.BccAddresses.Contains(";"))
                    {
                        emailTrackerDb.bcc_Addresses = string.Join(";", inputModel.BccAddresses);
                    }
                    else
                    {
                        emailTrackerDb.bcc_Addresses = inputModel.BccAddresses.FirstOrDefault();
                    }
                }

                return emailTrackerDb;
            }
            return null;
        }

        public email_tracker CreateModel<TModel>(email_tracker input, TModel model) where TModel : class
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TModel> CreateModelList<TModel>(IEnumerable<email_tracker> inputList) where TModel : class
        {
            var modelList = new List<EmailTracker>();
            foreach (var dbTask in inputList)
            {
                var emailTracker = new EmailTracker();
                emailTracker = CreateModel<EmailTracker>(dbTask);
                modelList.Add(emailTracker);
            }
            return modelList as IEnumerable<TModel>;
        }
    }
}

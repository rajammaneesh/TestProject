using DCode.Models.Common;
using DCode.Models.Email;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net.Mail;
using System.Threading.Tasks;

namespace DCode.Common
{
    public class EmailService : IEmailService
    {
        private readonly IAssetPathGeneratorFactory _assetPathGeneratorFactory;

        public EmailService(IAssetPathGeneratorFactory assetPathGeneratorFactory)
        {
            _assetPathGeneratorFactory = assetPathGeneratorFactory;
        }

        public void SendBulkEmail(IEnumerable<Notification> notifications)
        {
            Parallel.ForEach(notifications, new ParallelOptions
            {
                MaxDegreeOfParallelism = 4
            },
            notification =>
            {
                SendEmail(notification);
            });

        }

        private void SendEmail(Notification notification)
        {
            var mailMessage = new MailMessage();

            notification?.BccAddresses?.ForEach(address =>
            {
                mailMessage.Bcc.Add(address);
            });

            notification?.CcAddresses?.ForEach(address =>
            {
                mailMessage.CC.Add(address);
            });

            mailMessage.To.Add(notification?.ToAddresses);

            mailMessage.Subject = notification.Subject;

            mailMessage.IsBodyHtml = true;

            var pathGenerator = _assetPathGeneratorFactory.GetGenerator(PathGeneratorType.Notification);

            var inlineDCodeLogo = new LinkedResource(pathGenerator.GeneratePath(Constants.DCodeLogoPath));

            inlineDCodeLogo.ContentId = Guid.NewGuid().ToString();

            var inlineDeloitteLogo = new LinkedResource(pathGenerator.GeneratePath(Constants.Deloittepath));

            inlineDeloitteLogo.ContentId = Guid.NewGuid().ToString();

            mailMessage.Body =
                GetBodyForNotificationEmail(notification.Body,
                inlineDeloitteLogo.ContentId,
                inlineDCodeLogo.ContentId);

            using (var view = AlternateView.CreateAlternateViewFromString(mailMessage.Body, null, Constants.TextOrHtmlFormat))
            {
                view.LinkedResources.Add(inlineDCodeLogo);

                view.LinkedResources.Add(inlineDeloitteLogo);

                mailMessage.AlternateViews.Add(view);

                SendEmail(null, null, mailMessage);
            }
        }

        private string GetBodyForNotificationEmail(string body, string deloitteLogoId, string inlineTechId)
        {
            var htmlBody = GetEmail(PathGeneratorType.Notification);

            //Common implementation from here
            return string.Format(htmlBody, string.Empty, body, deloitteLogoId, inlineTechId);
        }

        public void SendEmail(string toMailAddress, string ccMailAddress, MailMessage mailMessage)
        {
            try
            {
                using (SmtpClient SmtpServer = new SmtpClient(Constants.SmtpDeloitte))
                {
                    mailMessage.From = new MailAddress(ConfigurationManager.AppSettings[Constants.DcodeEmailId]);

                    if (!string.IsNullOrWhiteSpace(toMailAddress))
                    {
                        mailMessage.To.Add(toMailAddress);
                    }

                    if (ccMailAddress != null)
                    {
                        if (ccMailAddress.Contains(";"))
                        {
                            var listAddresses = ccMailAddress.Split(';');
                            foreach (var address in listAddresses)
                            {
                                mailMessage.CC.Add(address);
                            }
                        }
                        else
                        {
                            mailMessage.CC.Add(ccMailAddress);
                        }
                    }

                    SmtpServer.Port = 25;
                    SmtpServer.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings[Constants.DcodeEmailId], ConfigurationManager.AppSettings[Constants.DcodeEmailPwd]);
                    SmtpServer.Send(mailMessage);

                }
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                return;
            }
        }

        public string GetEmail(PathGeneratorType generator)
        {
            var pathGenerator = _assetPathGeneratorFactory.GetGenerator(generator);

            string htmlBody = File.ReadAllText(pathGenerator.GeneratePath(Constants.EmailTemplatePath));

            return htmlBody;
        }
    }
}

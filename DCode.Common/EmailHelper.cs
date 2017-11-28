using System;
using System.Net.Mail;
using System.Configuration;
using System.Web.Hosting;
using DCode.Models.Enums;
using DCode.Models.Email;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using DCode.Models.Common;
using System.Threading;

namespace DCode.Common
{
    public static class EmailHelper
    {
        private static LinkedResource inlineDCodeLogo;

        private static LinkedResource inlineDeloitteLogo;
        public static void SendEmail(string toMailAddress, string ccMailAddress, MailMessage mailMessage)
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

        public static void SendApproveRejectNotification(string personName, string taskName, string projectName, Enums.EmailType type, string toMailAddress, string ccMailAddress)
        {
            var htmlBody = GetEmail(PathGeneratorType.Server);
            inlineDCodeLogo.ContentId = Guid.NewGuid().ToString();
            inlineDeloitteLogo.ContentId = Guid.NewGuid().ToString();
            using (var mailMessage = new MailMessage())
            {
                mailMessage.Subject = Constants.DCodeNotification;
                mailMessage.IsBodyHtml = true;
                var mainBody = string.Format(Constants.ApproveRejectBody, taskName, projectName, type.ToString());
                mailMessage.Body = string.Format(htmlBody, personName, mainBody, inlineDeloitteLogo.ContentId, inlineDCodeLogo.ContentId);
                using (var view = AlternateView.CreateAlternateViewFromString(mailMessage.Body, null, Constants.TextOrHtmlFormat))
                {
                    view.LinkedResources.Add(inlineDCodeLogo);
                    view.LinkedResources.Add(inlineDeloitteLogo);
                    mailMessage.AlternateViews.Add(view);

                    SendEmail(toMailAddress, ccMailAddress, mailMessage);
                }
            }
        }

        public static void AssignNotification(string personName, string taskName, string projectName, string wbsCode, string toMailAddress, string ccMailAddress)
        {
            var htmlBody = GetEmail(PathGeneratorType.Server);
            inlineDCodeLogo.ContentId = Guid.NewGuid().ToString();
            inlineDeloitteLogo.ContentId = Guid.NewGuid().ToString();
            using (var mailMessage = new MailMessage())
            {
                mailMessage.Subject = Constants.DCodeNotification;
                mailMessage.IsBodyHtml = true;
                var textBody = string.Format(Constants.AssignBody, taskName, projectName, projectName, wbsCode);
                mailMessage.Body = string.Format(htmlBody, personName, textBody, inlineDeloitteLogo.ContentId, inlineDCodeLogo.ContentId);
                using (var view = AlternateView.CreateAlternateViewFromString(mailMessage.Body, null, Constants.TextOrHtmlFormat))
                {
                    view.LinkedResources.Add(inlineDCodeLogo);
                    view.LinkedResources.Add(inlineDeloitteLogo);
                    mailMessage.AlternateViews.Add(view);

                    SendEmail(toMailAddress, ccMailAddress, mailMessage);
                }
            }
        }

        public static void ApplyNotification(string managerName, string personName, string taskName, string projectName, string hours, string startDateTime, string toMailAddress, string ccMailAddress)
        {
            var htmlBody = GetEmail(PathGeneratorType.Server);
            inlineDCodeLogo.ContentId = Guid.NewGuid().ToString();
            inlineDeloitteLogo.ContentId = Guid.NewGuid().ToString();
            using (var mailMessage = new MailMessage())
            {
                mailMessage.Subject = Constants.DCodeNotification;
                mailMessage.IsBodyHtml = true;
                var textBody = string.Format(Constants.ApplyBody, personName, taskName, projectName, hours, startDateTime);
                mailMessage.Body = string.Format(htmlBody, managerName, textBody, inlineDeloitteLogo.ContentId, inlineDCodeLogo.ContentId);
                using (var view = AlternateView.CreateAlternateViewFromString(mailMessage.Body, null, Constants.TextOrHtmlFormat))
                {
                    view.LinkedResources.Add(inlineDCodeLogo);
                    view.LinkedResources.Add(inlineDeloitteLogo);
                    mailMessage.AlternateViews.Add(view);

                    SendEmail(toMailAddress, ccMailAddress, mailMessage);
                }
            }
        }

        public static void ReviewNotification(string personName, string taskName, string projectName, string toMailAddress, string ccMailAddress)
        {
            var htmlBody = GetEmail(PathGeneratorType.Server);
            inlineDCodeLogo.ContentId = Guid.NewGuid().ToString();
            inlineDeloitteLogo.ContentId = Guid.NewGuid().ToString();

            using (var mailMessage = new MailMessage())
            {
                mailMessage.Subject = Constants.DCodeNotification;
                mailMessage.IsBodyHtml = true;
                var mainBody = string.Format(Constants.ReviewBody, taskName, projectName);
                mailMessage.Body = string.Format(htmlBody, personName, mainBody, inlineDeloitteLogo.ContentId, inlineDCodeLogo.ContentId);
                using (var view = AlternateView.CreateAlternateViewFromString(mailMessage.Body, null, Constants.TextOrHtmlFormat))
                {
                    view.LinkedResources.Add(inlineDCodeLogo);
                    view.LinkedResources.Add(inlineDeloitteLogo);
                    mailMessage.AlternateViews.Add(view);

                    SendEmail(toMailAddress, ccMailAddress, mailMessage);
                }
            }
        }

        public static string GetEmail(PathGeneratorType generator)
        {
            var pathGeneratorFactory = new AssetPathGeneratorFactory();

            var pathGenerator = pathGeneratorFactory.GetGenerator(generator);

            string htmlBody = File.ReadAllText(pathGenerator.GeneratePath(Constants.EmailTemplatePath));

            inlineDCodeLogo = new LinkedResource(pathGenerator.GeneratePath(Constants.DCodeLogoPath));

            inlineDCodeLogo.ContentId = Guid.NewGuid().ToString();

            inlineDeloitteLogo = new LinkedResource(pathGenerator.GeneratePath(Constants.Deloittepath));

            inlineDeloitteLogo.ContentId = Guid.NewGuid().ToString();

            return htmlBody;
        }

    }
}

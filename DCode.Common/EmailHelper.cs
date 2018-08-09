using System;
using System.Net.Mail;
using System.Configuration;
using DCode.Models.Enums;
using System.Collections.Generic;
using System.IO;
using DCode.Models.Common;
using System.Linq;

namespace DCode.Common
{
    public static class EmailHelper
    {
        private static LinkedResource inlineDCodeLogo;

        private static LinkedResource inlineDeloitteLogo;
        public static void SendEmail(string toMailAddress, string ccMailAddress, MailMessage mailMessage, List<string> bccAddress = null)
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

                    if (bccAddress != null && bccAddress.Any())
                    {
                        foreach (var address in bccAddress)
                        {
                            mailMessage.Bcc.Add(address);
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

        public static void SendApproveRejectNotification(string personName, string taskName, string projectName, Enums.EmailType type, string toMailAddress, string ccMailAddress, string offering)
        {
            var htmlBody = GetEmail(PathGeneratorType.Server);
            inlineDCodeLogo.ContentId = Guid.NewGuid().ToString();
            inlineDeloitteLogo.ContentId = Guid.NewGuid().ToString();
            using (var mailMessage = new MailMessage())
            {
                mailMessage.Subject = Constants.DCodeNotification;
                mailMessage.IsBodyHtml = true;
                var mainBody = string.Format(Constants.ApproveRejectBody, taskName, projectName, type.ToString(), offering);
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

        public static void AssignNotification(string personName, string taskName, string projectName, string wbsCode, string toMailAddress, string ccMailAddress, string offering)
        {
            var htmlBody = GetEmail(PathGeneratorType.Server);
            inlineDCodeLogo.ContentId = Guid.NewGuid().ToString();
            inlineDeloitteLogo.ContentId = Guid.NewGuid().ToString();
            using (var mailMessage = new MailMessage())
            {
                mailMessage.Subject = Constants.DCodeNotification;
                mailMessage.IsBodyHtml = true;
                var textBody = string.Format(Constants.AssignBody, taskName, projectName, projectName, wbsCode, offering);
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

        public static void ApplyNotification(string managerName, string personName, string taskName, string projectName, string hours, string startDateTime, string toMailAddress, string ccMailAddress, string offering)
        {
            var htmlBody = GetEmail(PathGeneratorType.Server);
            inlineDCodeLogo.ContentId = Guid.NewGuid().ToString();
            inlineDeloitteLogo.ContentId = Guid.NewGuid().ToString();
            using (var mailMessage = new MailMessage())
            {
                mailMessage.Subject = Constants.DCodeNotification;
                mailMessage.IsBodyHtml = true;
                var textBody = string.Format(Constants.ApplyBody, personName, taskName, projectName, hours, startDateTime, offering);
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

        public static void ApplyFINotification(string requestorName, string contributorName, string taskName, string taskDescription, string hours, string startDateTime, string toMailAddress, string ccMailAddress, string offering)
        {
            var htmlBody = GetEmail(PathGeneratorType.Server);
            inlineDCodeLogo.ContentId = Guid.NewGuid().ToString();
            inlineDeloitteLogo.ContentId = Guid.NewGuid().ToString();
            using (var mailMessage = new MailMessage())
            {
                mailMessage.Subject = Constants.DCodeNotification;
                mailMessage.IsBodyHtml = true;
                var textBody = string.Format(Constants.ApplyFIBody, contributorName, taskName, hours, startDateTime, offering);
                mailMessage.Body = string.Format(htmlBody, requestorName, textBody, inlineDeloitteLogo.ContentId, inlineDCodeLogo.ContentId);
                using (var view = AlternateView.CreateAlternateViewFromString(mailMessage.Body, null, Constants.TextOrHtmlFormat))
                {
                    view.LinkedResources.Add(inlineDCodeLogo);
                    view.LinkedResources.Add(inlineDeloitteLogo);
                    mailMessage.AlternateViews.Add(view);

                    SendEmail(toMailAddress, ccMailAddress, mailMessage);
                }
            }
        }

        public static void PostNewFINotification(string taskName, string hours, string description, string startDateTime, string ccMailAddress, List<string> bccMailAddress, string offering)
        {
            var htmlBody = GetEmail(PathGeneratorType.Server);
            inlineDCodeLogo.ContentId = Guid.NewGuid().ToString();
            inlineDeloitteLogo.ContentId = Guid.NewGuid().ToString();
            using (var mailMessage = new MailMessage())
            {
                mailMessage.Subject = string.Format(Constants.DCodeNewFINotification, taskName);
                mailMessage.IsBodyHtml = true;
                var textBody = string.Format(Constants.PostNewFIBody, taskName, hours, startDateTime, description, offering);
                mailMessage.Body = string.Format(htmlBody, "All", textBody, inlineDeloitteLogo.ContentId, inlineDCodeLogo.ContentId);
                using (var view = AlternateView.CreateAlternateViewFromString(mailMessage.Body, null, Constants.TextOrHtmlFormat))
                {
                    view.LinkedResources.Add(inlineDCodeLogo);
                    view.LinkedResources.Add(inlineDeloitteLogo);
                    mailMessage.AlternateViews.Add(view);

                    SendEmail(ConfigurationManager.AppSettings["DcodeEmailId"], ccMailAddress, mailMessage, bccMailAddress);
                }
            }
        }

        public static void ReviewNotification(string personName, string taskName, string projectName, string toMailAddress, string ccMailAddress, string offering)
        {
            var htmlBody = GetEmail(PathGeneratorType.Server);
            inlineDCodeLogo.ContentId = Guid.NewGuid().ToString();
            inlineDeloitteLogo.ContentId = Guid.NewGuid().ToString();

            using (var mailMessage = new MailMessage())
            {
                mailMessage.Subject = Constants.DCodeNotification;
                mailMessage.IsBodyHtml = true;
                var mainBody = string.Format(Constants.ReviewBody, taskName, projectName, offering);
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

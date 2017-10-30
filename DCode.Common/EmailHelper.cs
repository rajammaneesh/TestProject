using System;
using System.Net.Mail;
using System.Configuration;
using System.Web.Hosting;

namespace DCode.Common
{
    public static class EmailHelper
    {
        private static LinkedResource inlineDCodeLogo;
        private static LinkedResource inlineDeloitteLogo;
        public static void SendEmail(string toMailAddress,string ccMailAddress,MailMessage mailMessage)
        {
            try
            {
                MailMessage mail = mailMessage;
                SmtpClient SmtpServer = new SmtpClient(Constants.SmtpDeloitte);
                mail.From = new MailAddress(ConfigurationManager.AppSettings[Constants.DcodeEmailId]);
                mail.To.Add(toMailAddress);
                if (ccMailAddress != null)
                {
                    mail.CC.Add(ccMailAddress);
                }
                SmtpServer.Port = 25;
                SmtpServer.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings[Constants.DcodeEmailId], ConfigurationManager.AppSettings[Constants.DcodeEmailPwd]);
                SmtpServer.Send(mail);
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                return;
            }
        }

        public static void SendApproveRejectNotification(string personName, string taskName,string projectName,Enums.EmailType type,string toMailAddress,string ccMailAddress)
        {
            var htmlBody = GetEmail();
            inlineDCodeLogo.ContentId = Guid.NewGuid().ToString();
            inlineDeloitteLogo.ContentId = Guid.NewGuid().ToString();
            var mailMessage = new MailMessage();
            mailMessage.Subject = Constants.DCodeNotification;
            mailMessage.IsBodyHtml = true;
            var mainBody = string.Format(Constants.ApproveRejectBody,taskName,projectName,type.ToString());
            mailMessage.Body = string.Format(htmlBody, personName, mainBody, inlineDeloitteLogo.ContentId, inlineDCodeLogo.ContentId);
            var view = AlternateView.CreateAlternateViewFromString(mailMessage.Body, null,Constants.TextOrHtmlFormat);
            view.LinkedResources.Add(inlineDCodeLogo);
            view.LinkedResources.Add(inlineDeloitteLogo);
            mailMessage.AlternateViews.Add(view);

            SendEmail(toMailAddress, ccMailAddress,mailMessage);
        }

        public static void AssignNotification(string personName, string taskName, string projectName,string wbsCode, string toMailAddress, string ccMailAddress)
        {
            var htmlBody = GetEmail();
            inlineDCodeLogo.ContentId = Guid.NewGuid().ToString();
            inlineDeloitteLogo.ContentId = Guid.NewGuid().ToString();
            var mailMessage = new MailMessage();
            mailMessage.Subject = Constants.DCodeNotification;
            mailMessage.IsBodyHtml = true;
            var textBody = string.Format(Constants.AssignBody, taskName, projectName, projectName, wbsCode);
            mailMessage.Body = string.Format(htmlBody, personName, textBody, inlineDeloitteLogo.ContentId, inlineDCodeLogo.ContentId);
            var view = AlternateView.CreateAlternateViewFromString(mailMessage.Body, null, Constants.TextOrHtmlFormat);
            view.LinkedResources.Add(inlineDCodeLogo);
            view.LinkedResources.Add(inlineDeloitteLogo);
            mailMessage.AlternateViews.Add(view);

            SendEmail(toMailAddress, ccMailAddress, mailMessage);
        }

        public static void ApplyNotification(string managerName, string personName, string taskName, string projectName, string hours, string startDateTime, string toMailAddress, string ccMailAddress)
        {
            var htmlBody = GetEmail();
            inlineDCodeLogo.ContentId = Guid.NewGuid().ToString();
            inlineDeloitteLogo.ContentId = Guid.NewGuid().ToString();
            var mailMessage = new MailMessage();
            mailMessage.Subject = Constants.DCodeNotification;
            mailMessage.IsBodyHtml = true;
            //"{0} has requested to be assigned for {1} under project {2} for {3} starting {4}.<br/>He/she requires your permission to get assigned on this task.<br/>Kindly approve.<br/><br/>Regards,<br/>DCode Team<br/>Deloitte Digital";
            var textBody = string.Format(Constants.ApplyBody, personName, taskName, projectName, hours, startDateTime);
            mailMessage.Body = string.Format(htmlBody, managerName, textBody, inlineDeloitteLogo.ContentId, inlineDCodeLogo.ContentId);
            var view = AlternateView.CreateAlternateViewFromString(mailMessage.Body, null, Constants.TextOrHtmlFormat);
            view.LinkedResources.Add(inlineDCodeLogo);
            view.LinkedResources.Add(inlineDeloitteLogo);
            mailMessage.AlternateViews.Add(view);

            SendEmail(toMailAddress, ccMailAddress, mailMessage);
        }

        public static void ReviewNotification(string personName, string taskName, string projectName, string toMailAddress, string ccMailAddress)
        {
            var htmlBody = GetEmail();
            inlineDCodeLogo.ContentId = Guid.NewGuid().ToString();
            inlineDeloitteLogo.ContentId = Guid.NewGuid().ToString();

            var mailMessage = new MailMessage();
            mailMessage.Subject = Constants.DCodeNotification;
            mailMessage.IsBodyHtml = true;
            var mainBody = string.Format(Constants.ReviewBody,taskName,projectName);
            mailMessage.Body = string.Format(htmlBody, personName,mainBody , inlineDeloitteLogo.ContentId, inlineDCodeLogo.ContentId);
            var view = AlternateView.CreateAlternateViewFromString(mailMessage.Body, null, Constants.TextOrHtmlFormat);
            view.LinkedResources.Add(inlineDCodeLogo);
            view.LinkedResources.Add(inlineDeloitteLogo);
            mailMessage.AlternateViews.Add(view);

            SendEmail(toMailAddress, ccMailAddress, mailMessage);
        }

        public static string GetEmail()
        {
            string htmlBody = System.IO.File.ReadAllText(HostingEnvironment.MapPath(Constants.EmailTemplatePath));
            inlineDCodeLogo = new LinkedResource(HostingEnvironment.MapPath(Constants.DCodeLogoPath));
            inlineDCodeLogo.ContentId = Guid.NewGuid().ToString();
            inlineDeloitteLogo = new LinkedResource(HostingEnvironment.MapPath(Constants.Deloittepath));
            inlineDeloitteLogo.ContentId = Guid.NewGuid().ToString();
            return htmlBody;
        }
    }
}

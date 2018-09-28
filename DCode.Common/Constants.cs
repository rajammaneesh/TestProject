
using System.ComponentModel;

namespace DCode.Common
{
    public static class Constants
    {
        public const string DCode = "DCode";
        public const string DCodeEntities = "name=DCodeWebConString";
        public const string Insert = "Insert";
        public const string Update = "Update";
        public const string EmailId = "REQUESTOR_EMAIL_ID";
        public const string DaysAgo = " days ago";
        public const string DayAgo = " day ago";
        public const string HoursAgo = " hours ago";
        public const string HourAgo = " hour ago";
        public const string MinutesAgo = " minutes ago";
        public const string MinuteAgo = " minute ago";
        public const string Zero = "0";
        public const string ErrorRedirectType = "ErrorRedirect";
        public const string Location = "l";
        public const string Hyderabad = "hyderabad";
        public const string Delhi = "delhi";
        public const string Mumbai = "mumbai";
        public const string Bengaluru = "bengaluru";
        //Layout 
        public const string UrlDashboard = "dashboard";
        public const string UrlPermissions = "permissions";
        public const string UrlTaskLower = "task";
        public const string UrlHistory = "history";
        public const string TabMyTasks = "my tasks";
        public const string TabPermissions = "permissions";
        public const string TabNewTask = "new task";
        public const string TabHistory = "history";
        public const string PERMISSIONS = "APPROVALS";
        public const string CheckUsiAccess = "CheckUsiAccess";
        public const string ContactUS = "ContactUs";
        public const string UserManual = "User Manual";

        //Punctuation 
        public const string Comma = ",";
        public const string Space = " ";
        public const char SpaceChar = ' ';

        //UserContext Constats
        public const string UserContext = "DCodeUserContext";
        public const string UserContextMenuItems = "DCodeUserContextMenuItems";
        public const string SearchFilter = "(&(objectClass=user)(sAMAccountName={0}))";
        public const string LdapConnection = "LDAPConnection";
        public const string GenerateRedirectToError = "GenerateRedirectToError";
        public const string Userprincipalname = "userprincipalname";
        public const string Title = "title";
        public const string Givenname = "givenname";
        public const string SN = "sn";
        public const string Name = "name";
        public const string Mail = "mail";
        public const string DeloitteEmailExtn = "@deloitte.com";
        public const string EmployeeId = "employeeid";
        public const string Department = "department";
        public const string TelephoneNumber = "telephonenumber";
        public const string MsArchiveName = "msexcharchivename";
        public const string MockUser = "MockUser";
        public static string DcodeEmailId = "DcodeEmailId";
        public static string DcodeEmailPwd = "DcodeEmailPwd";
        public static string EmployeeType = "employeetype";
        public static string EnableTestFlow = "EnableTestFlow";
        public static string True = "true";
        public static string DCodeNotification = "TechX Notification";
        public static string DCodeNewFINotification = "New TechX FI Task Notification {0}";
        public static string UseWindowsIdentity = "UseWindowsIdentity";
        public static string SmtpDeloitte = "smtp.deloitte.com";
        public static string AssignBody = "You are assigned to work on task <b>{0}</b> under project <b>{1}</b> for the offering <b>{4}</b>.<br/><br/>WBS Code for project {2} - {3}<br/><br/>Regards,<br/>TechX Team";
        public static string ApproveRejectBody = "Your request to work on task <b>{0}</b> under project <b>{1}</b> for the offering <b>{3}</b> is <b>{2}</b>.<br/><br/>Regards,<br/>TechX Team";
        public static string ReviewBody = "Your task <b>{0}</b> under project <b>{1}</b> for the offering <b>{2}</b> is reviewed and closed.<br/><br/>Regards,<br/>TechX Team";
        public static string ApplyBody = "<b>{0}</b> has requested to be assigned for <b>{1}</b> under project <b>{2}</b> from the <b>{5}</b> offering for <b>{3}</b> starting <b>{4}</b>.<br/>He/she requires your permission to get assigned on this task.<br/>Kindly approve.<br/><br/>Regards,<br/>TechX Team";
        public static string ApplyFIBody = "<b>{0}</b> has requested to be assigned for the Firm Initiative <b>{1}</b> from the <b>{4}</b> offering for <b>{2}</b> hours starting <b>{3}</b>.<br/><br/>Regards,<br/>TechX Team";
        public static string PostNewFIBody = "A New Firm Initiative <b>{0}</b> from the <b>{4}</b> offering has been added to the TechX portal for <b>{1}</b> hours starting <b>{2}</b> by the lead (cc'ed).<br/><br/>Description {3} <br/><br/>Please apply for the task through TechX <br/><br/>Regards,<br/>TechX Team";
        public static string ClientServiceNotificationBody = "New Tasks have been added to the  <a href=\"http://techx/\">TechX</a> portal for the <b>{0}</b> skillset.{1}<br/><br/>Regards,<br/>TechX Team";
        public static string FirmInitiativeNotificationBody = "New Firm Initiatives have been added to the  <a href=\"http://techx/\">TechX</a> portal for the offering <b>{1}</b>.{0}<br/><br/>Regards,<br/>TechX Team";
        public static string CollectiveClientServiceTasksNotificationBody = "New Client Service Tasks have been added to the  <a href=\"http://techx/\">TechX</a> portal for the <b>{1}</b> offering.{0}<br/><br/>Regards,<br/>TechX Team";
        public static string TextOrHtmlFormat = "text/html";
        public static string EmailTemplatePath = "\\EmailTemplates\\email-template.html";
        public static string DCodeLogoPath = "\\Content\\Images\\tech-x-logo-black-bg.png";
        public static string Deloittepath = "\\Content\\Images\\Deloite_logo.png";
        public const string RMGroupEmailAddressKeyPrefix = "ServiceLineRMGroupEmail.";
        public const string FirmInitiativeSkillRecord = "Firm Initiative";
        public const string IndustryInitiativeSkillRecord = "Industry Initiative";
    }

    public static class ErrorMessages
    {
        public const string InvalidTaskId = "Invalid Task Id";
        public const string InvalidApplicantId = "Invalid Applicant Id";
        public const string TaskIsAssigned = "Task is already assigned";
        public const string SkillExists = "Skill exists!";
    }

    public static class SuccessMessages
    {
        public const string SkillAdded = "Added Skill";
    }
}

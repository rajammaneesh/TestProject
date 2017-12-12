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
        public static string UseWindowsIdentity = "UseWindowsIdentity";
        public static string SmtpDeloitte = "smtp.deloitte.com";
        public static string AssignBody = "You are assigned to work on task - {0} under project - {1}.<br/><br/>WBS Code for project {2} - {3}<br/><br/>Regards,<br/>TechX Team";
        public static string ApproveRejectBody = "Your request to work on task - {0} under project - {1} is {2}.<br/><br/>Regards,<br/>TechX Team";
        public static string ReviewBody = "Your task - {0} under project - {1} is reviewed and closed.<br/><br/>Regards,<br/>TechX Team";
        public static string ApplyBody = "{0} has requested to be assigned for {1} under project {2} for {3} starting {4}.<br/>He/she requires your permission to get assigned on this task.<br/>Kindly approve.<br/><br/>Regards,<br/>TechX Team";
        public static string NotificationBody = "New Tasks have been added to the  <a href=\"http://techx/\">TechX</a> portal for the {0} skillset.{1}<br/><br/>Regards,<br/>TechX Team";
        public static string TextOrHtmlFormat = "text/html";
        public static string EmailTemplatePath = "\\EmailTemplates\\email-template.html";
        public static string DCodeLogoPath = "\\Content\\Images\\tech-x-logo-black-bg.png";
        public static string Deloittepath = "\\Content\\Images\\Deloite_logo.png";
        public const string RMGroupEmailAddressKeyPrefix = "ServiceLineRMGroupEmail.";

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

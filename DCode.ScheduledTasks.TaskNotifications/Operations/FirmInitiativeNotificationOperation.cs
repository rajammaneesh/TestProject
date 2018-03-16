using DCode.Common;
using DCode.Models.Common;
using DCode.Models.Email;
using DCode.Models.Reporting;
using DCode.Services.Common;
using DCode.Services.Reporting;
using Ninject;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace DCode.ScheduledTasks.TaskNotifications.Operations
{
    public class FirmInitiativeNotificationOperation : IReportingExecutor
    {
        private readonly IReportingService _reportingService;

        private readonly IEmailService _emailService;

        private readonly ILoggerService _logService;

        private readonly INotificationContentFactory _notificationContentFactory;

        private readonly ITaskNotificationContent _notificationContent;

        public FirmInitiativeNotificationOperation(IKernel kernel)
        {
            _reportingService = kernel.Get<ReportingService>();

            _emailService = kernel.Get<EmailService>();

            _logService = kernel.Get<LoggerService>();

            _notificationContentFactory = kernel.Get<NotificationContentFactory>();

            _notificationContent =
                _notificationContentFactory.GetTaskNotificationContentGenerator(Models.Enums.Enums.TaskType.FirmInitiative);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void Invoke()
        {
            try
            {
                Console.WriteLine($"Application Started at {DateTime.Now.ToString()}");
                LogMessage($"Application Started at {DateTime.Now.ToString()}");

                IEnumerable<string> skills = null;

                if (skills == null
                    || skills.Count() == 0)
                {
                    Console.WriteLine($"No skills fetched from DB. Process Ended");
                    LogMessage($"No skills fetched from DB. Process Ended");

                    return;
                }

                Console.WriteLine($"Number skills fetched from DB = ");
                LogMessage($"Number skills fetched from DB = ");

                var registeredUsers = _reportingService.GetAllActiveUsers();

                var notifications = GetNotificationsForNewFirmInitiatives(registeredUsers, skills);

                Console.WriteLine($"Sending bulk emails");
                LogMessage($"Sending bulk emails");

                _emailService.SendBulkEmail(notifications);

                Console.WriteLine($"Sending bulk emails completed");
                LogMessage($"Sending bulk emails completed");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while executing :: {ex.StackTrace}");
                LogMessage($"An error occurred while executing :: {ex.StackTrace}");
            }
        }

        private IEnumerable<Notification> GetNotificationsForNewFirmInitiatives(
            IEnumerable<string> registeredUsers,
            IEnumerable<string> skills)
        {
            List<Notification> notifications = null;

            if (skills != null && skills.Any())
            {
                notifications = new List<Notification>();
            }

            var bodyRequest = new FirmInitiativeTaskNotificationContent
            {
                ProjectData = _reportingService.GetFirmInitiativeTasksCreatedYesterday()
            };

            notifications.AddRange(
                skills.Select(skill => new Notification
                {
                    BccAddresses = registeredUsers?.ToList(),
                    Skill = skill,
                    TaskDetails = null,// How do we implement this
                    ToAddresses = ConfigurationManager.AppSettings[Constants.DcodeEmailId],
                    Subject = _notificationContent.GetSubject(null),
                    Body = _notificationContent.GetEmailBody(bodyRequest)
                }));

            return notifications;
        }

        private void LogMessage(string description)
        {
            _logService.LogToDatabase(new Log
            {
                User = ApplicationConstants.ApplicationName,
                Description = description
            });
        }
    }
}

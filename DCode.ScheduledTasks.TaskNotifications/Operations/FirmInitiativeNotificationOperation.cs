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

        private readonly ITaskNotificationContent _notificationContentGenerator;

        public FirmInitiativeNotificationOperation(IKernel kernel)
        {
            _reportingService = kernel.Get<ReportingService>();

            _emailService = kernel.Get<EmailService>();

            _logService = kernel.Get<LoggerService>();

            _notificationContentFactory = kernel.Get<NotificationContentFactory>();

            _notificationContentGenerator =
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
                var registeredUsers = _reportingService.GetAllActiveUsers();

                Console.WriteLine($"Number of registered users ={registeredUsers.Count()}");
                LogMessage($"Number of registered users ={registeredUsers.Count()}");

                var firmInitiatives = _reportingService.GetFirmInitiativeTasksCreatedYesterday();

                if (firmInitiatives != null && firmInitiatives.Count() == 0)
                {
                    Console.WriteLine($"No firm initiatives from yesterday. Ending execution");
                    LogMessage($"No firm initiatives from yesterday. Ending execution");

                    return;
                }

                Console.WriteLine($"Number of firm initiatives ={firmInitiatives.Count()}");
                LogMessage($"Number of firm initiatives  ={firmInitiatives.Count()}");

                var notifications = GetNotificationsForNewFirmInitiatives(registeredUsers, firmInitiatives);

                Console.WriteLine($"Number of notifications ={notifications.Count()}");
                LogMessage($"Number of notifications  ={notifications.Count()}");

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
            IEnumerable<Tuple<string, string, string>> projectData)
        {
            List<Notification> notifications = null;

            if (projectData != null && projectData.Any())
            {
                notifications = new List<Notification>();
            }

            var bodyRequest = new FirmInitiativeTaskNotificationContent
            {
                ProjectData = projectData
            };

            notifications.Add(new Notification
            {
                BccAddresses = registeredUsers?.ToList(),
                ToAddresses = ConfigurationManager.AppSettings[Constants.DcodeEmailId],
                Subject = _notificationContentGenerator.GetSubject(null),
                Body = _notificationContentGenerator.GetEmailBody(bodyRequest)
            });

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

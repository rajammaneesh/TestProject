﻿using DCode.Common;
using DCode.Models.Common;
using DCode.Models.Email;
using DCode.Models.Reporting;
using DCode.ScheduledTasks.TaskNotifications;
using DCode.Services.Common;
using DCode.Services.Email;
using DCode.Services.Reporting;
using Ninject;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace DCode.ScheduledTasks.Operations.TaskNotifications
{
    internal class TaskNotificationsOperation : IReportingExecutor
    {
        private readonly IReportingService _reportingService;

        private readonly IEmailService _emailService;

        private readonly ILoggerService _logService;

        private readonly INotificationContentFactory _notificationContentFactory;

        private readonly ITaskNotificationContent _notificationContentType;

        public TaskNotificationsOperation(IKernel kernel)
        {
            _reportingService = kernel.Get<ReportingService>();

            _emailService = kernel.Get<EmailService>();

            _logService = kernel.Get<LoggerService>();

            _notificationContentFactory = kernel.Get<NotificationContentFactory>();

            _notificationContentType =
                _notificationContentFactory.GetTaskNotificationContentGenerator(
                    Models.Enums.Enums.TaskType.ClientService);
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

                var skills
                   = _reportingService.GetSkillsForNewTasksAddeddYesterday();

                if (skills == null
                    || skills.Count() == 0)
                {
                    Console.WriteLine($"No skills fetched from DB. Process Ended");
                    LogMessage($"No skills fetched from DB. Process Ended");

                    return;
                }

                Console.WriteLine($"Number skills fetched from DB = {skills.Count()}");
                LogMessage($"Number skills fetched from DB = {skills.Count()}");

                var notifications = GetNotificationsFromSkills(skills);

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

        private IEnumerable<Notification> GetNotificationsFromSkills(IEnumerable<string> skills)
        {
            List<Notification> notifications = null;

            if (skills != null && skills.Any())
            {
                notifications = new List<Notification>();
            }

            notifications.AddRange(skills
                ?.Select(skill =>
            {
                var recipients
                  = _reportingService.GetSubscribedUserForTask(skill);

                var projectInfo =
                    _reportingService.GetProjectDetailsForNewTasksAddedYesterday(skill);

                Console.WriteLine($"Number of recipients for {skill} is {(recipients != null ? recipients.Count() : 0)}");
                LogMessage($"Number of recipients for {skill} is {(recipients != null ? recipients.Count() : 0)}");

                var subjectRequest = new ClientServiceTaskNotificationSubject
                {
                    Skill = skill,
                };

                var contentRequest = new ClientServiceNotificationContent
                {
                    ProjectData = projectInfo,
                    Skill = skill
                };

                return new Notification
                {
                    BccAddresses = recipients?.ToList(),
                    Skill = skill,
                    ToAddresses = ConfigurationManager.AppSettings[Constants.DcodeEmailId],
                    Subject = _notificationContentType.GetSubject(subjectRequest),
                    Body = _notificationContentType.GetEmailBody(contentRequest)
                };
            }));

            Console.WriteLine($"Completed fetching recipients. Total recipient count is {notifications.Count}");
            LogMessage($"Completed fetching recipients. Total recipient count is {notifications.Count}");

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

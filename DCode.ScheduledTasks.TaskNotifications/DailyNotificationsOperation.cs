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

namespace DCode.ScheduledTasks.TaskNotifications
{
    internal class DailyNotificationsOperation : IReportingExecutor
    {
        private readonly IReportingService _reportingService;

        private readonly IEmailService _emailService;

        private readonly ILoggerService _logService;

        public DailyNotificationsOperation(IKernel kernel)
        {
            _reportingService = kernel.Get<ReportingService>();

            _emailService = kernel.Get<EmailService>();

            _logService = kernel.Get<LoggerService>();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void Invoke()
        {
            try
            {
                LogMessage($"Application Started at {DateTime.Now.ToString()}");

                var skills
                   = _reportingService.GetSkillsForNewTasksAddeddYesterday();

                if (skills == null
                    || skills.Count() == 0)
                {
                    LogMessage($"No skills fetched from DB. Process Ended");

                    return;
                }

                LogMessage($"Number skills fetched from DB = {skills.Count()}");

                var notifications = GetNotificationsFromSkills(skills);

                LogMessage($"Sending bulk emails");

                _emailService.SendBulkEmail(notifications);

                LogMessage($"Sending bulk emails completed");
            }
            catch (Exception ex)
            {
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

                LogMessage($"Number of recipients for {skill} is {(recipients != null ? recipients.Count() : 0)}");

                return new Notification
                {
                    BccAddresses = recipients?.ToList(),
                    Skill = skill,
                    ToAddresses = ConfigurationManager.AppSettings[Constants.DcodeEmailId],
                    TaskDetails = projectInfo
                };
            }));

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

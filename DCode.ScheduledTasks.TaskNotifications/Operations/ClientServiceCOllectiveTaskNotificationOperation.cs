using DCode.Common;
using DCode.Models.Common;
using DCode.Models.Email;
using DCode.Models.Reporting;
using DCode.Models.ResponseModels.Task;
using DCode.Services.Common;
using DCode.Services.Reporting;
using Ninject;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace DCode.ScheduledTasks.TaskNotifications.Operations
{
    public class ClientServiceCollectiveTaskNotificationOperation : IReportingExecutor
    {
        private readonly ICommonService _commonService;

        private readonly IReportingService _reportingService;

        private readonly ITaskNotificationContent _notificationContentGenerator;

        private readonly INotificationContentFactory _notificationContentFactory;

        private readonly IEmailService _emailService;

        public ClientServiceCollectiveTaskNotificationOperation(IKernel kernel)
        {
            _commonService = kernel.Get<CommonService>();

            _reportingService = kernel.Get<ReportingService>();

            _emailService = kernel.Get<EmailService>();

            _notificationContentFactory = kernel.Get<NotificationContentFactory>();

            _notificationContentGenerator =
              _notificationContentFactory.GetTaskNotificationContentGenerator(Models.Enums.Enums.TaskType.ClientServiceCollective);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void Invoke()
        {
            if (NeedsExecution())
            {
                var tasksCreatedFromDateRange =
                    _reportingService.GetNotificationsForCollectiveCSTasks(
                        GetExecutionDateRange());

                if (tasksCreatedFromDateRange == null)
                {
                    return;
                }

                var notifications = GetNotificationsFromTasks(tasksCreatedFromDateRange);

                if (notifications == null)
                {
                    return;
                }

                _emailService.SendBulkEmail(notifications);
            }
        }

        private bool NeedsExecution()
        {
            return DateTime.Now.DayOfWeek == DayOfWeek.Tuesday
                  || DateTime.Now.DayOfWeek == DayOfWeek.Thursday;
        }

        private int GetExecutionDateRange()
        {
            if (DateTime.Now.DayOfWeek == DayOfWeek.Tuesday)
            {
                return 4;
            }
            return 1;
        }

        private IEnumerable<Notification> GetNotificationsFromTasks(IEnumerable<Task> tasks)
        {
            var offerings = _commonService.GetOfferings();

            if (tasks == null || tasks.Count() == 0)
            {
                return null;
            }

            var notifications = new List<Notification>();

            tasks
                .Select(x => x.OfferingId)
                .Distinct()
                .ToList()
                .ForEach(offeringId =>
                {
                    var tasksForOffering = tasks.Where(x => x.OfferingId == offeringId);

                    var matchedOfferingRecord = offerings
                        .Where(x => x.Id == Convert.ToInt32(offeringId))
                        .FirstOrDefault();

                    var notificationBody = new ClientServiceCollectiveTaskNotificationContent
                    {
                        ProjectData = tasksForOffering
                            .Select(x => Tuple.Create<string, string, string>(x.TaskName, x.Details, x.FullName)),
                        OfferingName = matchedOfferingRecord.Description
                    };

                    var notificationSubject = new ClientServiceCollectiveTaskNotificationSubject
                    {
                        Offering = matchedOfferingRecord.Description
                    };

                    var practiceEmails = matchedOfferingRecord.GetPracticeEmailGroupsAsList();

                    practiceEmails = practiceEmails != null && practiceEmails.Any()
                        ? practiceEmails
                        : _commonService.GetDefaultConsultingMailboxes();

                    notifications.Add(new Notification
                    {
                        ToAddresses = ConfigurationManager.AppSettings[Constants.DcodeEmailId],

                        BccAddresses = practiceEmails,

                        Body = _notificationContentGenerator.GetEmailBody(notificationBody),

                        Subject = _notificationContentGenerator.GetSubject(notificationSubject)
                    });
                });

            return notifications;
        }
    }
}

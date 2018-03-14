using DCode.Common;
using DCode.Models.Common;
using DCode.Models.Reporting;
using DCode.Services.Common;
using DCode.Services.Reporting;
using Ninject;
using System;

namespace DCode.ScheduledTasks.TaskNotifications.Operations
{
    public class FirmInitiativeNotificationOperation : IReportingExecutor
    {
        private readonly IReportingService _reportingService;

        private readonly IEmailService _emailService;

        private readonly ILoggerService _logService;

        public FirmInitiativeNotificationOperation(IKernel kernel)
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
                Console.WriteLine($"Application Started at {DateTime.Now.ToString()}");
                LogMessage($"Application Started at {DateTime.Now.ToString()}");

                //Get skills for firm initiatives added yesterday

                //If skills is zero close the process

                Console.WriteLine($"Number skills fetched from DB = ");
                LogMessage($"Number skills fetched from DB = ");

                //Get all registered users

                //Get notification objects to be sent out

                Console.WriteLine($"Sending bulk emails");
                LogMessage($"Sending bulk emails");

                //Sed out bulk emails

                Console.WriteLine($"Sending bulk emails completed");
                LogMessage($"Sending bulk emails completed");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while executing :: {ex.StackTrace}");
                LogMessage($"An error occurred while executing :: {ex.StackTrace}");
            }
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

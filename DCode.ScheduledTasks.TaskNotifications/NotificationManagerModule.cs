using DCode.Common;
using DCode.Data.DbContexts;
using DCode.Data.LogRepository;
using DCode.Data.ReportingRepository;
using DCode.Data.TaskRepository;
using DCode.Data.UserRepository;
using DCode.Models.Common;
using DCode.Services.Common;
using DCode.Services.ModelFactory;
using DCode.Services.ModelFactory.CommonFactory;
using DCode.Services.Reporting;
using Ninject.Modules;

namespace DCode.ScheduledTasks.TaskNotifications
{
    internal class NotificationManagerModule : NinjectModule
    {
        public override void Load()
        {
            Bind(typeof(IUserRepository))
                .To(typeof(UserRepository))
                .InSingletonScope();

            Bind(typeof(ITaskRepository))
              .To(typeof(TaskRepository))
              .InSingletonScope();

            Bind(typeof(IReportingService))
                .To(typeof(ReportingService))
                .InSingletonScope();

            Bind(typeof(IEmailService))
             .To(typeof(EmailService))
             .InSingletonScope();

            Bind(typeof(IAssetPathGeneratorFactory))
                .To(typeof(AssetPathGeneratorFactory))
                .InSingletonScope();

            Bind(typeof(IModelFactory<log>))
             .To(typeof(LogModelFactory))
             .InSingletonScope();

            Bind(typeof(ILogRepository))
                .To(typeof(LogRepository))
                .InSingletonScope();

            Bind(typeof(ILoggerService))
                .To(typeof(LoggerService))
                .InSingletonScope();

            Bind(typeof(IDailyUsageStatisticsRepository))
              .To(typeof(DailyUsageStatisticsRepository))
              .InSingletonScope();
        }
    }
}

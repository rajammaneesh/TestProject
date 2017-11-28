using DCode.Data.TaskRepository;
using DCode.Data.UserRepository;
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
        }
    }
}

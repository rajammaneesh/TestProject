using Ninject;

namespace DCode.ScheduledTasks.TaskNotifications
{
    class Executable
    {
        static void Main(string[] args)
        {
            var kernel = new StandardKernel(new NotificationManagerModule());

            var operation = new DailyNotificationsOperation(kernel);

            operation.Invoke();

            System.Console.ReadKey();
        }
    }
}

using DCode.ScheduledTasks.Operations.TaskNotifications;
using DCode.ScheduledTasks.TaskNotifications.Operations;
using Ninject;
using System;

namespace DCode.ScheduledTasks.TaskNotifications
{
    public class Executable
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Entered App");

                var kernel = new StandardKernel(new NotificationManagerModule());

                Console.WriteLine("Initialized Kernel");

                var taskNotificationOperation = new TaskNotificationsOperation(kernel);

                var firmInitiativeNotificationOperation = new FirmInitiativeNotificationOperation(kernel);

                var clientServiceCollectiveTaskNotificationOperation = new ClientServiceCollectiveTaskNotificationOperation(kernel);

                Console.WriteLine("Initialized operation");

                Console.WriteLine("Initialized operation for client service notification");

                taskNotificationOperation.Invoke();

                Console.WriteLine("Ended operation for client service notification");

                Console.WriteLine("Initialized operation for firm initiative notification");

                firmInitiativeNotificationOperation.Invoke();

                Console.WriteLine("ended operation for firm initiative notification");

                Console.WriteLine("Initialized operation for client service collective task notification");

                clientServiceCollectiveTaskNotificationOperation.Invoke();

                Console.WriteLine("Ended operation for client service collective task notification");

                Console.WriteLine("Operation ended");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error to string:: " + ex.ToString());
                Console.WriteLine("Error::" + ex.StackTrace);
            }

            Console.ReadKey();
        }
    }
}

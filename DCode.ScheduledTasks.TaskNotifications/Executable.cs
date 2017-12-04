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

                var operation = new DailyNotificationsOperation(kernel);

                Console.WriteLine("Initialized operation");

                operation.Invoke();

                Console.WriteLine("Operation ended");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error::" + ex.StackTrace);
            }

            Console.ReadKey();
        }
    }
}

using static DCode.Models.Enums.Enums;

namespace DCode.Models.Common
{
    public interface INotificationContentFactory
    {
        ITaskNotificationContent GetTaskNotificationContentGenerator(TaskType type);
    }
}

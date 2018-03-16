using DCode.Services.Email;
using System;
using static DCode.Models.Enums.Enums;

namespace DCode.Models.Common
{
    public class NotificationContentFactory : INotificationContentFactory
    {
        private readonly IAssetPathGeneratorFactory _assetPathGeneratorFactory;

        public NotificationContentFactory(IAssetPathGeneratorFactory assetPathGeneratorFactory)
        {
            _assetPathGeneratorFactory = assetPathGeneratorFactory;
        }

        public ITaskNotificationContent GetTaskNotificationContentGenerator(TaskType type)
        {
            switch (type)
            {
                case TaskType.ClientService:
                    return new ClientServiceTaskNotificationContentGenerator(_assetPathGeneratorFactory);

                case TaskType.FirmInitiative:
                    return new FirmInitiativeTaskNotificationContentGenerator(_assetPathGeneratorFactory);

                default:
                    throw new ArgumentException($"Invalid notification content Generated");

            }
        }
    }
}

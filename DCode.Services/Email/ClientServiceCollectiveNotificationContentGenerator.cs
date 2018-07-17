using DCode.Models.Common;
using System;
using DCode.Models.Email;
using System.IO;
using DCode.Common;

namespace DCode.Services.Email
{
    public class ClientServiceCollectiveNotificationContentGenerator : ITaskNotificationContent
    {
        private readonly IAssetPathGeneratorFactory _assetPathGeneratorFactory;

        public ClientServiceCollectiveNotificationContentGenerator(
            IAssetPathGeneratorFactory assetPathGeneratorFactory)
        {
            _assetPathGeneratorFactory = assetPathGeneratorFactory;
        }

        public string GetDynamicTableContent(INotificationContent content)
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            var tableContent = content as ClientServiceCollectiveTaskNotificationContent;

            var tableHtml = string.Empty;

            tableHtml += $"<tr><th style='border:1px solid #4d4d4d;text-align:left;padding:8px;'>Task Name</th><th style='border:1px solid #4d4d4d;text-align:left;padding:8px;'>Task Description</th></tr>";

            foreach (var item in tableContent.ProjectData)
            {
                tableHtml += $"<tr><td style='border:1px solid #4d4d4d;text-align:left;padding:8px;'>{item.Item1}</td><td style='border:1px solid #4d4d4d;text-align:left;padding:8px;'>{item.Item2}</td></tr>";
            }

            var finalTableHtml = $"<table style='border-collapse:collapse;width:80%;'>{tableHtml}</table>";

            return $"<p>{finalTableHtml}</p>";
        }

        public string GetEmailBody(INotificationContent content)
        {
            var taskContent = content as ClientServiceCollectiveTaskNotificationContent;

            var pathGenerator = _assetPathGeneratorFactory.GetGenerator(PathGeneratorType.Notification);

            string htmlBody = File.ReadAllText(pathGenerator.GeneratePath(Constants.EmailTemplatePath));

            var mainBody =
              string.Format(Constants.CollectiveClientServiceTasksNotificationBody, GetDynamicTableContent(content));

            return mainBody;
        }

        public string GetSubject(ITaskNotificationSubject subject)
        {
            if (subject == null)
            {
                throw new ArgumentNullException(nameof(subject));
            }

            var subjectObject = subject as ClientServiceCollectiveTaskNotificationSubject;

            return $"TX :: New CS Tasks available for {subjectObject.Offering} offering";
        }
    }
}

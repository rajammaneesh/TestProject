using DCode.Common;
using DCode.Models.Common;
using DCode.Models.Email;
using System;
using System.IO;

namespace DCode.Services.Email
{
    public class ClientServiceTaskNotificationContentGenerator : ITaskNotificationContent
    {
        private readonly IAssetPathGeneratorFactory _assetPathGeneratorFactory;

        public ClientServiceTaskNotificationContentGenerator(IAssetPathGeneratorFactory assetPathGeneratorFactory)
        {
            _assetPathGeneratorFactory = assetPathGeneratorFactory;
        }

        public string GetDynamicTableContent(INotificationContent dynamicContent)
        {
            if (dynamicContent == null)
            {
                throw new ArgumentNullException(nameof(dynamicContent));
            }

            var tableContent = dynamicContent as ClientServiceNotificationContent;

            var tableHtml = string.Empty;

            tableHtml += $"<tr><th style='border:1px solid #4d4d4d;text-align:left;padding:8px;'>Project Name</th><th style='border:1px solid #4d4d4d;text-align:left;padding:8px;'> Task Name</th></tr>";

            foreach (var item in tableContent.ProjectData)
            {
                tableHtml += $"<tr><td style='border:1px solid #4d4d4d;text-align:left;padding:8px;'>{item.Item1}</td><td style='border:1px solid #4d4d4d;text-align:left;padding:8px;'>{item.Item2}</td></tr>";
            }

            var finalTableHtml = $"<table style='border-collapse:collapse;width:80%;'>{tableContent}</table>";

            return $"<p>{tableHtml}</p>";
        }

        public string GetEmailBody(INotificationContent content)
        {
            var taskContent = content as ClientServiceNotificationContent;

            var pathGenerator = _assetPathGeneratorFactory.GetGenerator(PathGeneratorType.Notification);

            string htmlBody = File.ReadAllText(pathGenerator.GeneratePath(Constants.EmailTemplatePath));

            var mainBody =
              string.Format(Constants.ClientServiceNotificationBody, taskContent.Skill, GetDynamicTableContent(content));

            return mainBody;
        }

        public string GetSubject(ITaskNotificationSubject subject)
        {
            if (subject == null)
            {
                throw new ArgumentNullException(nameof(subject));
            }

            var subjectObject = subject as ClientServiceTaskNotificationSubject;

            return $"TechX :: New Task available for {subjectObject} skillset";
        }
    }
}

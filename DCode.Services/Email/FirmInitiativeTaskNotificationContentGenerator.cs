using System;
using DCode.Models.Common;
using DCode.Models.Email;
using System.IO;
using DCode.Common;

namespace DCode.Services.Email
{
    public class FirmInitiativeTaskNotificationContentGenerator : ITaskNotificationContent
    {
        private readonly IAssetPathGeneratorFactory _assetPathGeneratorFactory;

        public FirmInitiativeTaskNotificationContentGenerator(IAssetPathGeneratorFactory assetPathGeneratorFactory)
        {
            _assetPathGeneratorFactory = assetPathGeneratorFactory;
        }

        public string GetDynamicTableContent(INotificationContent dynamicContent)
        {
            if (dynamicContent == null)
            {
                throw new ArgumentNullException(nameof(dynamicContent));
            }

            var tableContent = dynamicContent as FirmInitiativeTaskNotificationContent;

            var tableHtml = string.Empty;

            tableHtml += $"<tr><th style='border:1px solid #4d4d4d;text-align:left;padding:8px;'>Firm Initiative Name</th><th style='border:1px solid #4d4d4d;text-align:left;padding:8px;'>Firm Initiative Details</th><th style='border:1px solid #4d4d4d;text-align:left;padding:8px;'>Skill</th></tr>";

            foreach (var item in tableContent.ProjectData)
            {
                tableHtml += $"<tr><td style='border:1px solid #4d4d4d;text-align:left;padding:8px;'>{item.Item1}</td><td style='border:1px solid #4d4d4d;text-align:left;padding:8px;'>{item.Item2}</td><td style='border:1px solid #4d4d4d;text-align:left;padding:8px;'>{item.Item3}</td></tr>";
            }

            var finalTableHtml = $"<table style='border-collapse:collapse;width:80%;'>{tableHtml}</table>";

            return $"<p>{finalTableHtml}</p>";
        }

        public string GetEmailBody(INotificationContent content)
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            var pathGenerator = _assetPathGeneratorFactory.GetGenerator(PathGeneratorType.Notification);

            var fiContent = content as FirmInitiativeTaskNotificationContent;

            string htmlBody = File.ReadAllText(pathGenerator.GeneratePath(Constants.EmailTemplatePath));

            var mainBody =
              string.Format(Constants.FirmInitiativeNotificationBody, GetDynamicTableContent(content), fiContent.OfferingName);

            return mainBody;
        }

        public string GetSubject(ITaskNotificationSubject subject)
        {
            return $"New Firm Initiatives available on TX";
        }
    }
}

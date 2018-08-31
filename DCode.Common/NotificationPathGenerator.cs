using System.IO;
using System.Reflection;

namespace DCode.Models.Common
{
    public class NotificationPathGenerator : IAssetPathGenerator
    {
        public string GeneratePath(string location)
        {
            location = location.Replace("/", "\\");

            return $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}{location}";
        }
    }
}

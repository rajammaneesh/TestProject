using System.Web.Hosting;

namespace DCode.Models.Common
{
    public class ServerPathGenerator : IAssetPathGenerator
    {
        public string GeneratePath(string location)
        {
            location = location.Replace("\\", "/");

            if (!location.StartsWith("~"))
            {
                location = location.Insert(0, "~");
            }

            return HostingEnvironment.MapPath(location);
        }
    }
}

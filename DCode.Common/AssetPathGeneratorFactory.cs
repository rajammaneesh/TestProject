namespace DCode.Models.Common
{
    public class AssetPathGeneratorFactory : IAssetPathGeneratorFactory
    {
        public IAssetPathGenerator GetGenerator(PathGeneratorType generator)
        {
            switch (generator)
            {
                case PathGeneratorType.Server:
                    return new ServerPathGenerator();

                case PathGeneratorType.Notification:
                    return new NotificationPathGenerator();

                default:
                    return null;
            }
        }
    }
}

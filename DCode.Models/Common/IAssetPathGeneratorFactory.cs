namespace DCode.Models.Common
{
    public interface IAssetPathGeneratorFactory
    {
        IAssetPathGenerator GetGenerator(PathGeneratorType generator);
    }
}
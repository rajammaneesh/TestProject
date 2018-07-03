using DCode.Data.DbContexts;
using System.Collections.Generic;

namespace DCode.Data.MetadataRepository
{
    public interface IPortfolioRepository
    {
        IEnumerable<portfolio> GetPortfolios();
    }
}

using DCode.Data.DbContexts;
using System.Collections.Generic;

namespace DCode.Data.MetadataRepository
{
    public interface IServiceLineRepository
    {
        IEnumerable<service_line> GetServiceLines();
    }
}

using DCode.Data.DbContexts;
using System.Collections.Generic;

namespace DCode.Data.MetadataRepository
{
    public interface IOfferingRepository
    {
        IEnumerable<offering> GetOfferings();

        int UpdateOffering(offering offering);
    }
}

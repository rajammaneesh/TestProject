using DCode.Data.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCode.Data.MetadataRepository
{
    public interface ISubOfferingRepository
    {
        IEnumerable<suboffering> GetSubOfferings(int offeringId);

        suboffering GetSubOfferingById(int subOfferingId);
    }
}

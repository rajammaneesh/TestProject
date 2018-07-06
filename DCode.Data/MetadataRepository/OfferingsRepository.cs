using System.Collections.Generic;
using DCode.Data.DbContexts;
using DCode.Data.Repository;
using System.Linq;

namespace DCode.Data.MetadataRepository
{
   public class OfferingsRepository: Repository<offering>, IOfferingsRepository
    {
        private readonly OfferingDbContext _context;

        public OfferingsRepository(OfferingDbContext context)
            : base(context)
        {
            _context = context;
        }

        public IEnumerable<offering> GetOfferings()
        {
            return Context.Set<offering>().ToList();
        }
    }
}

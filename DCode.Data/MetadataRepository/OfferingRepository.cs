using System.Collections.Generic;
using DCode.Data.DbContexts;
using DCode.Data.Repository;
using System.Linq;
using System.Linq.Dynamic;
using System.Data.Entity;

namespace DCode.Data.MetadataRepository
{
    public class OfferingRepository : Repository<offering>, IOfferingRepository
    {
        private readonly MetadataDbContext _context;

        public OfferingRepository(MetadataDbContext context)
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

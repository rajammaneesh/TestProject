using System.Collections.Generic;
using DCode.Data.DbContexts;
using DCode.Data.Repository;
using System.Linq;
using System.Linq.Dynamic;
using System.Data.Entity;

namespace DCode.Data.MetadataRepository
{
    public class ServiceLineRepository : Repository<service_line>, IServiceLineRepository
    {
        private readonly MetadataDbContext _context;

        public ServiceLineRepository(MetadataDbContext context)
            : base(context)
        {
            _context = context;
        }

        public IEnumerable<service_line> GetServiceLines()
        {
            return Context.Set<service_line>().ToList();
        }
    }
}

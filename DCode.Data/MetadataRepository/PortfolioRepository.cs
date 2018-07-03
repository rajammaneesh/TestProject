using System.Collections.Generic;
using DCode.Data.DbContexts;
using DCode.Data.Repository;
using System.Linq;
using System.Linq.Dynamic;
using System.Data.Entity;

namespace DCode.Data.MetadataRepository
{
    public class PortfolioRepository : Repository<portfolio>, IPortfolioRepository
    {
        private readonly MetadataDbContext _context;

        public PortfolioRepository(MetadataDbContext context)
            : base(context)
        {
            _context = context;
        }

        public IEnumerable<portfolio> GetPortfolios()
        {
            return Context.Set<portfolio>().ToList();
        }
    }
}

using System.Collections.Generic;
using DCode.Data.DbContexts;
using DCode.Data.Repository;
using System.Linq;
using System.Linq.Dynamic;
using System.Data.Entity;

namespace DCode.Data.MetadataRepository
{
    public class ApprovedApplicantRepository : Repository<approvedapplicant>, IApprovedApplicantRepository
    {
        private readonly MetadataDbContext _context;

        public ApprovedApplicantRepository(MetadataDbContext context)
            : base(context)
        {
            _context = context;
        }

        public IEnumerable<approvedapplicant> GetApprovedApplicants()
        {
            return Context.Set<approvedapplicant>().ToList();
        }
    }
}

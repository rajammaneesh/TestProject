using System.Collections.Generic;
using DCode.Data.DbContexts;
using DCode.Data.Repository;
using System.Linq;
using System.Linq.Dynamic;
using System.Data.Entity;
using System;

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

        public int UpdateOffering(offering offering)
        {
            var result = Context.Set<offering>().FirstOrDefault(x => x.Id == offering.Id);
            if (result != null)
            {
                result.Code = offering.Code;
                result.Description = offering.Description;
                result.Id = offering.Id;
                result.Portfolio_Id = offering.Portfolio_Id;
                result.Practice_Email_Group = offering.Practice_Email_Group;
                result.RM_Email_Group = offering.RM_Email_Group;
            }
            return Context.SaveChanges();
        }
    }
}

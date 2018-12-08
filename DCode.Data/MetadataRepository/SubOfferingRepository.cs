using System;
using System.Collections.Generic;
using System.Linq;
using DCode.Data.DbContexts;
using DCode.Data.Repository;
using System.Text;
using System.Threading.Tasks;

namespace DCode.Data.MetadataRepository
{
    public class SubOfferingRepository: Repository<suboffering>, ISubOfferingRepository
    {
        private readonly MetadataDbContext _context;

        public SubOfferingRepository(MetadataDbContext context)
            : base(context)
        {
            _context = context;
        }

        /// <summary>
        /// Get the subofferings by offeringId
        /// </summary>
        /// <param name="offeringId"></param>
        /// <returns></returns>
        public IEnumerable<suboffering> GetSubOfferings(int offeringId)
        {
            return Context.Set<suboffering>().Where(item => item.OfferingsId == offeringId);
        }

        /// <summary>
        /// Get the subofferings by SubofferingId
        /// </summary>
        /// <param name="offeringId"></param>
        /// <returns></returns>
        public suboffering GetSubOfferingById(int subOfferingId)
        {
            return Context.Set<suboffering>().Where(item => item.ID == subOfferingId).FirstOrDefault();
        }
    }
}

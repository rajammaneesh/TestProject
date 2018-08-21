using System.Collections.Generic;
using DCode.Data.DbContexts;
using DCode.Data.Repository;
using System.Linq;

namespace DCode.Data.MetadataRepository
{
    public class UserPointsRepository : Repository<approvedapplicant>, IUserPointsRepository
    {
        private readonly MetadataDbContext _context;

        public UserPointsRepository(MetadataDbContext context)
            : base(context)
        {
            _context = context;
        }

        public IEnumerable<user_points> GetUserPoints()
        {
            return Context.Set<user_points>().ToList();
        }

        public int InsertUserPoints(user_points pointsRecord)
        {
            var record = Context.Set<user_points>().Add(pointsRecord);

            return Context.SaveChanges();
        }
    }
}

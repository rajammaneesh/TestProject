using System.Collections.Generic;
using DCode.Data.DbContexts;
using DCode.Data.Repository;
using System.Linq;
using System.Data.Entity;

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
            IQueryable<user_points> userPointsRecords = Context.Set<user_points>();

            userPointsRecords.Include(x => x.user).Load();

            userPointsRecords.Include(x => x.user_role).Load();

            return userPointsRecords.ToList();
        }

        public int GetUserPointsForUser(int userId, int roleId)
        {
            var userPoints = Context.Set<user_points>()
                .Where(x => x.user_id == userId
                    && x.role_id == roleId)
                ?.Select(x => x.points)
                ?.Sum();

            return userPoints ?? 0;
        }

        public int InsertUserPoints(user_points pointsRecord)
        {
            var record = Context.Set<user_points>().Add(pointsRecord);

            return Context.SaveChanges();
        }
    }
}

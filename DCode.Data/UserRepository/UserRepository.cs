using System.Collections.Generic;
using DCode.Data.DbContexts;
using DCode.Data.Repository;
using System.Linq;
using static DCode.Models.Enums.Enums;

namespace DCode.Data.UserRepository
{
    public class UserRepository : Repository<user>, IUserRepository
    {
        private readonly UserDbContext _context;

        public UserRepository(UserDbContext context)
            : base(context)
        {
            _context = context;
        }

        public IEnumerable<string> GetSubscribedUserForTask(string task)
        {
            IQueryable<user> query = Context.Set<user>()
                 .Where(i => (i.notification_subscription.Count > 0 && i.notification_subscription.First().subscription_status == true)
                    && i.applicantskills.Any(y => y.skill.VALUE.Trim() == task.Trim())
                    && i.STATUS == UserStatus.Active.ToString());

            return query?.Select(i => i.EMAIL_ID)?.ToList();
        }
    }
}

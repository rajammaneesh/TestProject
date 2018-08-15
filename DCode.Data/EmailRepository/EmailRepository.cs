using DCode.Data.DbContexts;
using DCode.Data.Repository;
using System;

namespace DCode.Data.EmailRepository
{
    public class EmailRepository : Repository<emailtracker>, IEmailRepository
    {
        private readonly EmailDbContext _context;

        public EmailRepository(EmailDbContext context)
            : base(context)
        {
            _context = context;
        }

        public int InsertEmailDetails(emailtracker emailTracker)
        {
            try
            {
                var insertedTask = Context.Set<emailtracker>().Add(emailTracker);
                return Context.SaveChanges();
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
    }
}

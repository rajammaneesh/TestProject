using DCode.Data.DbContexts;
using DCode.Data.Repository;
using System;

namespace DCode.Data.EmailRepository
{
    public class EmailRepository : Repository<email_tracker>, IEmailRepository
    {
        private readonly EmailDbContext _context;

        public EmailRepository(EmailDbContext context)
            : base(context)
        {
            _context = context;
        }

        public int InsertEmailDetails(email_tracker emailTracker)
        {
            try
            {
                var insertedTask = Context.Set<email_tracker>().Add(emailTracker);
                return Context.SaveChanges();
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
    }
}

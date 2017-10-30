using DCode.Data.DbContexts;
using DCode.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCode.Data.LogRepository
{
    public class LogRepository : Repository<log>,  ILogRepository
    {
        private readonly LogDbContext _context;

        public LogRepository(LogDbContext context)
            : base(context)
        {
            _context = context;
        }

        public int InsertLog(log logModel)
        {
            try
            {
                var insertedTask =  Context.Set<log>().Add(logModel);
                return Context.SaveChanges();
            }
            catch(Exception ex)
            {
                return 0;
            }
            return 0;
        }

        public IEnumerable<log> GetLogs()
        {
            return Context.Set<log>().OrderByDescending(x => x.Id).ToList();
        }
    }
}

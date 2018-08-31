using System;
using System.Collections.Generic;
using DCode.Data.DbContexts;
using DCode.Data.Repository;
using System.Linq;

namespace DCode.Data.ReportingRepository
{
    public class DailyUsageStatisticsRepository :
        Repository<daily_usage_statistics>,
        IDailyUsageStatisticsRepository
    {
        private readonly ReportingDbContext _context;

        public DailyUsageStatisticsRepository(ReportingDbContext context)
            : base(context)
        {
            _context = context;
        }

        public IEnumerable<daily_usage_statistics> GetDailyStatisticsFor()
        {
            return GetDailyStatisticsHistory().Take(100);
        }

        public IEnumerable<daily_usage_statistics> GetDailyStatisticsHistory()
        {
            return Context.Set<daily_usage_statistics>();
        }

        public void UpsertDailyStatistics()
        {
            var currentDate = DateTime.Now;

            var dateToCompare = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day);

            var statisticsTableRecords = Context.Set<daily_usage_statistics>()
                 .Where(x => x.date == dateToCompare);

            if (statisticsTableRecords != null && statisticsTableRecords.Any())
            {
                var countRecord = statisticsTableRecords.First();

                countRecord.visits = Convert.ToInt32(countRecord.visits) + 1;
            }
            else
            {
                Context.Set<daily_usage_statistics>().Add(new daily_usage_statistics
                {
                    date = dateToCompare,
                    visits = 1
                });
            }

            Context.SaveChanges();
        }
    }
}

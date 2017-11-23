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
        public IEnumerable<daily_usage_statistics> GetDailyStatisticsFor(DateTime date)
        {
            return GetDailyStatisticsHistory()
                 .Where(x => x.date.Date == date.Date);
        }

        public IEnumerable<daily_usage_statistics> GetDailyStatisticsHistory()
        {
            return Context.Set<daily_usage_statistics>();
        }

        public void UpsertDailyStatistics()
        {
            var statisticsTableRecords = Context.Set<daily_usage_statistics>()
                 .Where(x => x.date.Date == DateTime.Now.Date);

            if (statisticsTableRecords != null && statisticsTableRecords.Any())
            {
                statisticsTableRecords.First().visits = statisticsTableRecords.First().visits++;
            }
            else
            {
                Context.Set<daily_usage_statistics>().Add(new daily_usage_statistics
                {
                    date = DateTime.Now,
                    visits = 1
                });
            }

            Context.SaveChanges();
        }
    }
}

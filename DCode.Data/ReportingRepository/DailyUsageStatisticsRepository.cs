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
    }
}

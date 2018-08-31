using DCode.Data.DbContexts;
using System;
using System.Collections.Generic;

namespace DCode.Data.ReportingRepository
{
    public interface IDailyUsageStatisticsRepository
    {
        IEnumerable<daily_usage_statistics> GetDailyStatisticsHistory();

        IEnumerable<daily_usage_statistics> GetDailyStatisticsFor();

        void UpsertDailyStatistics();
    }
}

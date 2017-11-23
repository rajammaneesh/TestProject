using System;
using System.Collections.Generic;

namespace DCode.Services.Reporting
{
    public interface IReportingService
    {
        void UpdateDailySiteVisitCount();

        List<Tuple<DateTime, int>> GetUserVisitsCount(int noOfRecords = 1);
    }
}

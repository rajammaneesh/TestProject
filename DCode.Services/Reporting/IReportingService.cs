using System;
using System.Collections.Generic;

namespace DCode.Services.Reporting
{
    public interface IReportingService
    {
        IEnumerable<string> GetSubscribedUserForTask(string task);

        IEnumerable<string> GetSkillsForNewTasksAddeddYesterday();

        IEnumerable<Tuple<string, string>> GetProjectDetailsForNewTasksAddedYesterday(string skill);

        void UpdateDailySiteVisitCount();

        List<Tuple<DateTime, int>> GetUserVisitsCount(int noOfRecords = 1);
    }
}

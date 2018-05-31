using DCode.Models.Common;
using System;
using System.Collections.Generic;

namespace DCode.Services.Reporting
{
    public interface IReportingService
    {
        IEnumerable<string> GetSubscribedUserForTask(string task);

        IEnumerable<string> GetSkillsForNewTasksAddeddYesterday();

        IEnumerable<Tuple<string, string>> GetProjectDetailsForNewTasksAddedYesterday(string skill);

        IEnumerable<Tuple<string, string, string, int>> GetFirmInitiativeTasksCreatedYesterday();

        void UpdateDailySiteVisitCount();

        List<Tuple<string, long>> GetUserVisitsCount();

        DatabaseTable ExecuteDbQuery(string query);

        IEnumerable<string> GetConsultingUsersForServiceLine(int serviceLineId);

        IEnumerable<string> GetDummyConsultingUsers();
    }
}

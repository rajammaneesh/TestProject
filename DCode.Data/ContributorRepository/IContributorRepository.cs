using DCode.Data.DbContexts;
using System.Collections.Generic;

namespace DCode.Data.ContributorRepository
{
    public interface IContributorRepository
    {
        IEnumerable<taskskill> GetTasksBasedOnApplicantSkills(IEnumerable<int> skillIds, int userId);
        int ApplyForTask(taskapplicant taskApplicant);

        IEnumerable<approvedapplicant> GetAssignedTask(int userId);
        IEnumerable<approvedapplicant> GetTaskStatus(int userId);
        IEnumerable<taskapplicant> GetAppliedTasks(int userId);
        int UpdateHours(int approvedApplicantId, int hours);
        IEnumerable<task> GetAllTasks(int currentPageIndex, int recordsCount, out int totalRecords);
        IEnumerable<taskskill> GetTasksBasedOnSkill(string skill, int currentPageIndex, int recordsCount, out int totalRecords);
        IEnumerable<taskskill> GetFilteredTasks(List<string> skillFilters, string serviceLine, string searchText, int currentPageIndex, int recordsCount, out int totalRecords);
        IEnumerable<approvedapplicant> GetTaskHistories(int userId, int currentPageIndex, int recordsCount, out int totalRecords);
        IEnumerable<approvedapplicant> GetAssignedTask(int userId, int currentPageIndex, int recordsCount, out int totalRecordsCount);
    }



}

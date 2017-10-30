using DCode.Data.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        IEnumerable<taskskill> GetTasksBasedOnSkillOrDescription(string filter, int currentPageIndex, int recordsCount, out int totalRecords);
        IEnumerable<approvedapplicant> GetTaskHistories(int userId, int currentPageIndex, int recordsCount, out int totalRecords);
        IEnumerable<approvedapplicant> GetAssignedTask(int userId, int currentPageIndex, int recordsCount, out int totalRecordsCount);
    }
}

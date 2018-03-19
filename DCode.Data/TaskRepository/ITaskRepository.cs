using DCode.Data.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCode.Data.TaskRepository
{
    public interface ITaskRepository
    {
        int InsertTask(task task, IEnumerable<taskskill> taskSkills);
        task GetTaskById(int id);
        int UpdateTask(task task);
        IEnumerable<task> GetTasks();
        //IEnumerable<task> GetTasks(int pageCount, int totalRecords);
        //applicant GetApplicantById(int id);
        int GetTopRatingCountOnEmailId(string emailId);
        IEnumerable<task> GetTaskByEmailId(string emailId);
        //int InsertApplicant(applicant applicant);
        IEnumerable<task> GetTaskHistroyByEmailId(string emailId);
        IEnumerable<skill> GetAllSkills();

        List<KeyValuePair<string, string>> GetAllCommentsOnEmailId(string emailId);
        IEnumerable<task> GetTaskHistroyByEmailId(string emailId, int currentPageIndex, int recordsCount, out int totalRecords);

        IEnumerable<Tuple<string, int>> GetTaskCountBySkillForDate(DateTime date);

        IEnumerable<task> GetFirmInitiativesForDate(DateTime date);

        IEnumerable<task> GetProjectDetailsForNewTasksFromDateForSkill(DateTime date, string skillName);
    }
}

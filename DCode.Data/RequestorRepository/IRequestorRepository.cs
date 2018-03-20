using DCode.Data.DbContexts;
using System.Collections.Generic;
using static DCode.Models.Enums.Enums;

namespace DCode.Data.RequestorRepository
{
    public interface IRequestorRepository
    {
        IEnumerable<task> GetTaskApplicantsForApproval(int selectedTaskTypeId, int currentPageIndex, int recordsCount, string emailId, out int totalRecords);
        int AssignTask(approvedapplicant dbApprovedApplicant);
        //IEnumerable<approvedapplicant> GetApprovedApplicantsByTaskId(int taskId);
        IEnumerable<approvedapplicant> GetStatusOftasks(string emailId, int currentPageIndex, int recordsCount, TaskStatusSortFields sortField, SortOrder sortOrder, out int totalRecords);
        //approvedapplicant GetApprovedApplicantById(int id);
        int ReviewTask(approvedapplicant approvedApplicant);
        //IEnumerable<task> GetTaskHistroyByEmailId(string emailId);
        //IEnumerable<applicant> GetTasksForManagerToApprove(string emailId);
        //IEnumerable<task> GetCompletedTasks(string emailId);
        IEnumerable<taskapplicant> GetTaskApplicantsForPermissions(int currentPageIndex, int recordsCount, string emailId, out int totalRecords);
        int AllowTask(taskapplicant taskApplicant);
        int RejectTask(taskapplicant taskApplicant);
        user GetUserByEmailId(string emailId);
        IEnumerable<skill> SearchSkill(string searchParam);
        taskapplicant GetTaskApplicantByApplicantId(int applicantId);
        approvedapplicant GetApprovedApplicant(int approvedAppId);
        int GetPermissionsCount(string emailId);
    }
}

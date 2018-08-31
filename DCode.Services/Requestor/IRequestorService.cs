using DCode.Models.RequestModels;
using DCode.Models.ResponseModels.Requestor;
using DCode.Models.ResponseModels.Task;
using System.Collections.Generic;
using static DCode.Models.Enums.Enums;

namespace DCode.Services.Requestor
{
    public interface IRequestorService
    {
        TaskApplicantsReponse GetTaskApplicantsForApproval(int selectedTaskTypeId, int currentPageIndex, int recordsCount);
        int AssignTask(AssignTaskRequest taskRequest);
        //IEnumerable<ApprovedContributor> GetApprovedApplicantsByTaskId(int taskId);
        TaskStatusResponse GetStatusOftasks(int selectedTaskType, int currentPageIndex, int recordsCount, TaskStatusSortFields sortField, SortOrder sortOrder);
        int ReviewTask(ReviewTaskRequest reviewTaskRequest);
        IEnumerable<TaskHistory> GetTaskHistory();
        bool IsFirstTimeUserForNewTask();
        //bool IsFirstTimeForPermission();
        //bool IsFirstTimeForTaskHistory();
        PermissionTaskResponse GetTaskApplicantsForPermissions(int currentPageIndex, int recordsCount);
        int AllowTask(ApproveTaskRequest taskRequest);
        int RejectTask(RejectTaskRequest rejectTaskRequest);
        TaskHistoryResponse GetTaskHistories(int currentPageIndex, int recordsCount);
        int GetPermissionsCount();

    }
}

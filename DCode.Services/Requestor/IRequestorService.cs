using DCode.Common;
using DCode.Data.DbContexts;
using DCode.Models;
using DCode.Models.RequestModels;
using DCode.Models.ResponseModels.Common;
using DCode.Models.ResponseModels.Contributor;
using DCode.Models.ResponseModels.Requestor;
using DCode.Models.ResponseModels.Task;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DCode.Services.Requestor
{
    public interface IRequestorService
    {
        TaskApplicantsReponse GetTaskApplicantsForApproval(int currentPageIndex, int recordsCount);
        int AssignTask(AssignTaskRequest taskRequest);
        //IEnumerable<ApprovedContributor> GetApprovedApplicantsByTaskId(int taskId);
        TaskStatusResponse GetStatusOftasks(int currentPageIndex, int recordsCount, Enums.TaskStatusSortFields sortField, Enums.SortOrder sortOrder);
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

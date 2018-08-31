using DCode.Models.ResponseModels.Contributor;
using DCode.Models.ResponseModels.Requestor;
using DCode.Models.ResponseModels.Task;
using System;
using System.Collections.Generic;

namespace DCode.Services.Contributor
{
    public interface IContributorService
    {
        IEnumerable<Models.ResponseModels.Task.Task> GetTasksBasedOnApplicantSkills();
        IEnumerable<TaskHistory> GetTaskHistory();
        int ApplyTask(int taskId, string emailAddress, string statementOfPurpose);

        //AssignedTasksResponse GetApprovedTasksForCurrentUser();
        AssignedTasksResponse GetApprovedTasksForCurrentUser(int currentPageIndex, int recordsCount);
        int UpdateHours(int approvedApplicantId, int hours);
        TaskResponse GetAllTasks(string searchKey, int currentPageIndex, int recordsCount,string searchFilter, int selectedTaskType);
        TaskHistoryResponse GetTaskHistories(int currentPageIndex, int recordsCount);
        int ApplyFITask(int taskId,string requestor);
    }
}

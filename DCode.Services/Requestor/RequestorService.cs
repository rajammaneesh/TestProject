using System;
using System.Collections.Generic;
using System.Linq;
using DCode.Data.TaskRepository;
using DCode.Data.DbContexts;
using DCode.Services.ModelFactory;
using DCode.Data.RequestorRepository;
using DCode.Models.ResponseModels.Requestor;
using DCode.Models.ResponseModels.Task;
using DCode.Models.ResponseModels.Contributor;
using DCode.Common;
using DCode.Models.RequestModels;
using DCode.Services.Base;
using DCode.Services.Common;
using DCode.Models.ResponseModels.Common;
using static DCode.Models.Enums.Enums;

namespace DCode.Services.Requestor
{
    public class RequestorService : BaseService, IRequestorService
    {
        private ITaskRepository _taskRepository;
        private TaskModelFactory _taskModelFactory;
        private TaskApplicantModelFactory _taskApplicantModelFactory;
        private ApplicantModelFactory _applicantModelFactory;
        private ApprovedApplicantModelFactory _approvedApplicantModelFactory;
        private IRequestorRepository _requestorRepository;
        private ApprovedContributorModelFactory _approvedContributorModelFactory;
        private SkillModelFactory _skillModelFactory;
        private ICommonService _commonService;
        private IUserRepository _userRepository;
        public RequestorService(ITaskRepository taskRepository, TaskModelFactory taskModelFactory, IRequestorRepository requestorRepository, ApplicantModelFactory applicantModelFactory, ApprovedContributorModelFactory approvedContributorModelFactory, ICommonService commonService, TaskApplicantModelFactory taskApplicantModelFactory, ApprovedApplicantModelFactory approvedApplicantModelFactory, IUserRepository userRepository, SkillModelFactory skillModelFactory)
        {
            _taskRepository = taskRepository;
            _taskModelFactory = taskModelFactory;
            _requestorRepository = requestorRepository;
            _applicantModelFactory = applicantModelFactory;
            _approvedContributorModelFactory = approvedContributorModelFactory;
            _commonService = commonService;
            _taskApplicantModelFactory = taskApplicantModelFactory;
            _approvedApplicantModelFactory = approvedApplicantModelFactory;
            _userRepository = userRepository;
            _skillModelFactory = skillModelFactory;
        }

        public TaskApplicantsReponse GetTaskApplicantsForApproval(int currentPageIndex, int recordsCount)
        {
            var start = DateTime.Now;
            var response = new TaskApplicantsReponse();
            var taskApprovals = new List<TaskApproval>();
            var totalRecords = 0;
            IEnumerable<task> dbTaskApprovals;
            //If Admin - TBD after usercontext module
            //if (!true)
            var user = _commonService.GetCurrentUserContext();
            if (!true)
            {
                dbTaskApprovals = _requestorRepository.GetTaskApplicantsForApproval(currentPageIndex, recordsCount, string.Empty, out totalRecords);
            }
            else
            {
                dbTaskApprovals = _requestorRepository.GetTaskApplicantsForApproval(currentPageIndex, recordsCount, user.EmailId, out totalRecords);
            }
            foreach (var dbTaskApproval in dbTaskApprovals)
            {
                var taskApproval = new TaskApproval();
                taskApproval.Task = _taskModelFactory.CreateModel<Models.ResponseModels.Task.Task>(dbTaskApproval);
                taskApproval.Applicants = new List<Models.ResponseModels.Contributor.Contributor>();
                taskApproval.TaskApplicantId = dbTaskApproval.ID;
                foreach (var dbApplicant in dbTaskApproval.taskapplicants)
                {
                    if (dbApplicant.STATUS == ApplicantStatus.ManagerApproved.ToString())
                    {
                        var applicant = _taskApplicantModelFactory.CreateModel<Models.ResponseModels.Contributor.Contributor>(dbApplicant);

                        applicant.TopRatingsCount = _taskRepository.GetTopRatingCountOnEmailId(applicant.EmailId);

                        applicant.Expertise = ConvertSkillsToString(_userRepository.GetSkillsByUserId(applicant.ApplicantId));

                        applicant.StatementOfPurpose = dbApplicant.STATEMENT_OF_PURPOSE ?? string.Empty;

                        applicant.Comments =
                            _taskRepository
                            .GetAllCommentsOnEmailId(applicant.EmailId)
                            .Select(x => new ManagerComments
                            {
                                ManagerId = x.Key,
                                Comment = x.Value
                            })
                            ?.ToList();

                        taskApproval.Applicants.Add(applicant);
                    }
                }
                taskApprovals.Add(taskApproval);
            }

            response.TaskApprovals = taskApprovals;

            response.TotalRecords = totalRecords;

            var end = DateTime.Now - start;

            return response;
        }

        //private List<TaskApproval> ProcessApplicantForTaskApprovals(IList<taskapplicant> taskApplicants)
        //{

        //}

        public int AssignTask(AssignTaskRequest taskRequest)
        {
            var userContext = _commonService.GetCurrentUserContext();
            var dbApprovedApplicant = new approvedapplicant();
            dbApprovedApplicant.ID = taskRequest.TaskApplicantId;
            dbApprovedApplicant.TASK_ID = taskRequest.TaskId;
            dbApprovedApplicant.APPLICANT_ID = taskRequest.ApplicantId;
            MapAuditFields<approvedapplicant>(ActionType.Insert, dbApprovedApplicant);
            var result = _requestorRepository.AssignTask(dbApprovedApplicant);
            if (result > 0)
            {
                var task = _taskRepository.GetTaskById(taskRequest.TaskId);
                var applicant = _requestorRepository.GetTaskApplicantByApplicantId(taskRequest.TaskApplicantId);
                EmailHelper.AssignNotification(applicant.user.FIRST_NAME + Constants.Space + applicant.user.LAST_NAME, applicant.task.TASK_NAME, applicant.task.PROJECT_NAME, applicant.task.PROJECT_WBS_Code, applicant.user.EMAIL_ID, userContext.EmailId+ ";" + applicant.user.MANAGER_EMAIL_ID);
            }
            return result;
        }

        public TaskStatusResponse GetStatusOftasks(int currentPageIndex, int recordsCount, TaskStatusSortFields sortField, SortOrder sortOrder)
        {
            var user = _commonService.GetCurrentUserContext();
            var response = new TaskStatusResponse();
            var totalRecords = 0;
            var taskStatusList = new List<Models.ResponseModels.Requestor.TaskStatus>();
            var dbApprovedApplicants = _requestorRepository.GetStatusOftasks(user.EmailId, currentPageIndex, recordsCount, TaskStatusSortFields.Name, SortOrder.DESC, out totalRecords);
            foreach (var dbApprovedApplicant in dbApprovedApplicants)
            {
                var taskStatus = new Models.ResponseModels.Requestor.TaskStatus();
                taskStatus.Applicant = _approvedApplicantModelFactory.CreateModel<DCode.Models.ResponseModels.Contributor.Contributor>(dbApprovedApplicant);
                taskStatus.Task = _taskModelFactory.CreateModel<DCode.Models.ResponseModels.Task.Task>(dbApprovedApplicant.task);
                taskStatus.ApprovedApplicantId = dbApprovedApplicant.ID;
                taskStatus.Duration = CommonHelper.CalculateDuration(dbApprovedApplicant.CREATED_ON);
                taskStatusList.Add(taskStatus);
            }
            response.TaskStatuses = taskStatusList;
            response.TotalRecords = totalRecords;
            return response;
        }

        public int ReviewTask(ReviewTaskRequest reviewTaskRequest)
        {
            var userContext = _commonService.GetCurrentUserContext();
            var approvedApplicant = _approvedApplicantModelFactory.CreateModel<ReviewTaskRequest>(reviewTaskRequest);
            MapAuditFields<approvedapplicant>(ActionType.Update, approvedApplicant);
            approvedApplicant.APPLICANT_ID = reviewTaskRequest.ApplicantId;
            approvedApplicant.ID = reviewTaskRequest.ApprovedApplicantId;
            approvedApplicant.RATING = reviewTaskRequest.Rating;
            approvedApplicant.COMMENTS = reviewTaskRequest.Comments;
            approvedApplicant.WORK_AGAIN = reviewTaskRequest.WorkAgain;
            var status = _requestorRepository.ReviewTask(approvedApplicant);
            if (status > 0)
            {
                var approved = _requestorRepository.GetTaskApplicantByApplicantId(approvedApplicant.ID);
            }
            return status;
        }

        public IEnumerable<TaskHistory> GetTaskHistory()
        {
            var tasksHistory = new List<TaskHistory>();
            var user = _commonService.GetCurrentUserContext();
            var dbTasksHistory = _taskRepository.GetTaskHistroyByEmailId(user.EmailId);
            foreach (var dbtask in dbTasksHistory)
            {
                var task = new TaskHistory();
                task.Task = _taskModelFactory.CreateModel<DCode.Models.ResponseModels.Task.Task>(dbtask);
                task.Applicant = _taskApplicantModelFactory.CreateModel<ContributorSummary>(dbtask.taskapplicants.FirstOrDefault());
                tasksHistory.Add(task);
            }
            return tasksHistory;
        }

        public TaskHistoryResponse GetTaskHistories(int currentPageIndex, int recordsCount)
        {
            var taskHistoryResponse = new TaskHistoryResponse();
            var tasksHistory = new List<TaskHistory>();
            var user = _commonService.GetCurrentUserContext();
            var totalRecords = 0;
            var dbTasks = _taskRepository.GetTaskHistroyByEmailId(user.EmailId, currentPageIndex, recordsCount, out totalRecords);
            taskHistoryResponse.TotalRecords = totalRecords;
            foreach (var dbTask in dbTasks)
            {
                var task = new TaskHistory();
                task.Task = _taskModelFactory.CreateModel<DCode.Models.ResponseModels.Task.Task>(dbTask);
                task.Applicant = _taskApplicantModelFactory.CreateModel<ContributorSummary>(dbTask.taskapplicants.FirstOrDefault());
                tasksHistory.Add(task);
            }
            taskHistoryResponse.TaskHistories = tasksHistory;
            return taskHistoryResponse;
        }

        public PermissionTaskResponse GetTaskApplicantsForPermissions(int currentPageIndex, int recordsCount)
        {
            var user = _commonService.GetCurrentUserContext();
            var start = DateTime.Now;
            var response = new PermissionTaskResponse();
            var permissionTaskList = new List<PermissionsTask>();
            var totalRecords = 0;
            IEnumerable<taskapplicant> dbTaskApplicants;
            //If Admin - TBD after usercontext module
            //if (!true)
            if (!true)
            {
                dbTaskApplicants = _requestorRepository.GetTaskApplicantsForPermissions(currentPageIndex, recordsCount, string.Empty, out totalRecords);
            }
            else
            {
                dbTaskApplicants = _requestorRepository.GetTaskApplicantsForPermissions(currentPageIndex, recordsCount, user.EmailId, out totalRecords);
            }
            foreach (var dbTaskApp in dbTaskApplicants)
            {
                var permissionsTask = new PermissionsTask();
                permissionsTask.Applicant = _taskApplicantModelFactory.CreateModel<DCode.Models.ResponseModels.Contributor.Contributor>(dbTaskApp);
                permissionsTask.Task = _taskModelFactory.CreateModel<DCode.Models.ResponseModels.Task.Task>(dbTaskApp.task);
                permissionsTask.TaskApplicantId = dbTaskApp.ID;
                permissionTaskList.Add(permissionsTask);
            }
            response.permissionTasks = permissionTaskList;
            response.TotalRecords = totalRecords;
            var end = DateTime.Now - start;
            return response;
        }

        public int AllowTask(ApproveTaskRequest taskRequest)
        {
            var userContext = _commonService.GetCurrentUserContext();
            var dbApplicant = new taskapplicant();
            dbApplicant.ID = taskRequest.TaskApplicantId;
            dbApplicant.APPLICANT_ID = taskRequest.ApplicantId;
            dbApplicant.TASK_ID = taskRequest.TaskId;
            MapAuditFields<taskapplicant>(ActionType.Update, dbApplicant);
            var result = _requestorRepository.AllowTask(dbApplicant);
            if (result > 0)
            {
                var task = _taskRepository.GetTaskById(taskRequest.TaskId);
                var applicant = _requestorRepository.GetTaskApplicantByApplicantId(taskRequest.TaskApplicantId);
                EmailHelper.SendApproveRejectNotification(applicant.user.FIRST_NAME + Constants.Space + applicant.user.LAST_NAME, applicant.task.TASK_NAME, applicant.task.PROJECT_NAME, EmailType.Approved, applicant.user.EMAIL_ID, userContext.EmailId);
            }

            return result;
        }

        public int RejectTask(RejectTaskRequest rejectTaskRequest)
        {
            var userContext = _commonService.GetCurrentUserContext();
            var taskapplicant = new taskapplicant();
            taskapplicant.ID = rejectTaskRequest.TaskApplicantId;
            taskapplicant.APPLICANT_ID = rejectTaskRequest.ApplicantId;
            taskapplicant.TASK_ID = rejectTaskRequest.TaskId;
            MapAuditFields<taskapplicant>(ActionType.Update, taskapplicant);
            var status = _requestorRepository.RejectTask(taskapplicant);
            if (status > 0)
            {
                var task = _taskRepository.GetTaskById(rejectTaskRequest.TaskId);
                var applicant = _requestorRepository.GetTaskApplicantByApplicantId(rejectTaskRequest.TaskApplicantId);
                EmailHelper.SendApproveRejectNotification(applicant.user.FIRST_NAME + Constants.Space + applicant.user.LAST_NAME, applicant.task.TASK_NAME, applicant.task.PROJECT_NAME, EmailType.Rejected, applicant.user.EMAIL_ID, userContext.EmailId);
            }
            return status;
        }

        public bool IsFirstTimeUserForNewTask()
        {
            var user = _commonService.GetCurrentUserContext();
            var tasks = _taskRepository.GetTaskByEmailId(user.EmailId);
            return tasks != null ? tasks.Count() == 0 : false;
        }

        public int GetPermissionsCount()
        {
            var userContext = _commonService.GetCurrentUserContext();
            return _requestorRepository.GetPermissionsCount(userContext.EmailId);
        }

    }
}

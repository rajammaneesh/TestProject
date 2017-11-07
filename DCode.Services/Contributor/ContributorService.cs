using DCode.Common;
using DCode.Data.ContributorRepository;
using DCode.Data.DbContexts;
using DCode.Data.RequestorRepository;
using DCode.Data.TaskRepository;
using DCode.Models.ResponseModels.Contributor;
using DCode.Models.ResponseModels.Requestor;
using DCode.Models.ResponseModels.Task;
using DCode.Services.Base;
using DCode.Services.Common;
using DCode.Services.ModelFactory;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;


namespace DCode.Services.Contributor
{
    public class ContributorService : BaseService, IContributorService
    {
        private ITaskRepository _taskRepository;
        private IContributorRepository _contributorRepository;
        private TaskSkillModelFactory _taskSkillModelFactory;
        private TaskModelFactory _taskModelFactory;
        private TaskApplicantModelFactory _taskApplicantModelFactory;
        //private ApplicantModelFactory _applicantModelFactory;
        private ApprovedApplicantModelFactory _approvedApplicantModelFactory;
        //private IRequestorRepository _requestorRepository;
        //private ApprovedContributorModelFactory _approvedContributorModelFactory;
        private ICommonService _commonService;
        private IUserRepository _userRepository;
        public ContributorService(ITaskRepository taskRepository, ICommonService commonService, IContributorRepository contributorRepository, TaskSkillModelFactory taskSkillModelFactory, TaskModelFactory taskModelFactory, ApprovedApplicantModelFactory approvedApplicantModelFactory, IUserRepository userRepository)//, TaskModelFactory taskModelFactory, IRequestorRepository requestorRepository, ApplicantModelFactory applicantModelFactory, ApprovedContributorModelFactory approvedContributorModelFactory, ICommonService commonService, TaskApplicantModelFactory taskApplicantModelFactory, ApprovedApplicantModelFactory approvedApplicantModelFactory, IUserRepository userRepository)
        {
            _taskRepository = taskRepository;
            _contributorRepository = contributorRepository;
            _taskSkillModelFactory = taskSkillModelFactory;
            _taskModelFactory = taskModelFactory;
            _approvedApplicantModelFactory = approvedApplicantModelFactory;
            //_requestorRepository = requestorRepository;
            //_applicantModelFactory = applicantModelFactory;
            //_approvedContributorModelFactory = approvedContributorModelFactory;
            _commonService = commonService;
            //_taskApplicantModelFactory = taskApplicantModelFactory;
            _approvedApplicantModelFactory = approvedApplicantModelFactory;
            _userRepository = userRepository;
        }

        public IEnumerable<Models.ResponseModels.Task.Task> GetTasksBasedOnApplicantSkills()
        {
            var user = _commonService.GetCurrentUserContext();
            var skillList = new List<int>();
            if (user != null && user.SkillSet != null)
            {
                foreach (var skill in user.SkillSet)
                {
                    skillList.Add(skill.Id);
                }
                var dbtaskSkills = _contributorRepository.GetTasksBasedOnApplicantSkills(skillList, user.UserId);
                var dbTaskApplicants = _contributorRepository.GetAppliedTasks(user.UserId);
                var tasks = _taskSkillModelFactory.CreateModelList<Models.ResponseModels.Task.Task>(dbtaskSkills);
                //Map Applied field
                foreach (var appliedTask in dbTaskApplicants)
                {
                    var taskObj = tasks.Where(x => x.Id == appliedTask.TASK_ID).FirstOrDefault();
                    if (taskObj != null)
                    {
                        taskObj.IsApplied = true;
                    }
                }
                return tasks;
            }
            return null;
        }

        public IEnumerable<TaskHistory> GetTaskHistory()
        {
            var tasksHistory = new List<TaskHistory>();
            var user = _commonService.GetCurrentUserContext();
            var dbapprovedapplicants = _contributorRepository.GetTaskStatus(user.UserId);

            foreach (var dbapprovedapplicant in dbapprovedapplicants)
            {
                var task = new TaskHistory();
                task.Task = _taskModelFactory.CreateModel<DCode.Models.ResponseModels.Task.Task>(dbapprovedapplicant.task);
                task.Applicant = _approvedApplicantModelFactory.CreateModel<ContributorSummary>(dbapprovedapplicant);
                tasksHistory.Add(task);
            }
            return tasksHistory;
        }

        public int ApplyTask(int taskId, string emailAddress, string statementOfPurpose)
        {
            try
            {
                var user = _commonService.GetCurrentUserContext();

                var taskApplicant = new taskapplicant
                {
                    APPLICANT_ID = user.UserId,
                    TASK_ID = taskId,
                    STATUS = Enums.ApplicantStatus.Active.ToString(),
                    STATUS_DATE = DateTime.Now,
                    STATEMENT_OF_PURPOSE = statementOfPurpose
                };

                MapAuditFields(Enums.ActionType.Insert, taskApplicant);

                var result = _contributorRepository.ApplyForTask(taskApplicant);

                if (result > 0)
                {
                    var task = _taskRepository.GetTaskById(taskId);

                    if (string.IsNullOrWhiteSpace(emailAddress))
                    {
                        emailAddress = user.ManagerEmailId;
                    }

                    var managerName = _commonService.GetNameFromEmailId(emailAddress);

                    _commonService.UpdateManagersEmail(user.EmailId, emailAddress, managerName);

                    var department = user.Department;

                    var serviceLines = _commonService.GetServiceLines();

                    var currentUsersServiceLine = string.Empty;
                    foreach(var serviceLine in serviceLines)
                    {
                        if(department.Split(' ').Contains(serviceLine.Name))
                        {
                            currentUsersServiceLine = serviceLine.Name;
                            break;
                        }
                    }

                    var RMGroupEmailAddress = ConfigurationManager.AppSettings[Constants.RMGroupEmailAddressKeyPrefix + currentUsersServiceLine];

                    EmailHelper.ApplyNotification(
                        managerName,
                        $"{user.FirstName}{Constants.Space}{user.LastName}",
                        task.TASK_NAME,
                        task.PROJECT_NAME,
                        $"{task.HOURS.ToString()}h",
                        task.ONBOARDING_DATE.Value.ToShortDateString(),
                        emailAddress,
                        $"{user.EmailId};{RMGroupEmailAddress}");


                }
                return result;
            }
            catch (Exception ex)
            {
                var logDetails = new DCode.Models.Common.Log { Description = ex.Message, Details = ex.ToString() };
                _commonService.LogToDatabase(logDetails);
                return -1;
            }
        }


        public AssignedTasksResponse GetApprovedTasksForCurrentUser(int currentPageIndex, int recordsCount)
        {
            var user = _commonService.GetCurrentUserContext();
            var response = new AssignedTasksResponse();
            var totalRecords = 0;
            var taskStatusList = new List<AssignedTask>();
            //var dbApprovedApplicants = _contributorRepository.GetAssignedTask(user.UserId).Where(x => x.task.STATUS == Enums.TaskStatus.Assigned.ToString() && x.user.ID == user.UserId);
            var dbApprovedApplicants = _contributorRepository.GetAssignedTask(user.UserId, currentPageIndex, recordsCount, out totalRecords);
            response.TotalRecords = totalRecords;
            foreach (var dbApprovedApplicant in dbApprovedApplicants)
            {
                var taskStatus = new AssignedTask();
                taskStatus.Applicant = _approvedApplicantModelFactory.CreateModel<DCode.Models.ResponseModels.Contributor.Contributor>(dbApprovedApplicant);
                taskStatus.Task = _taskModelFactory.CreateModel<DCode.Models.ResponseModels.Task.Task>(dbApprovedApplicant.task);
                taskStatus.ApprovedApplicantId = dbApprovedApplicant.ID;
                taskStatusList.Add(taskStatus);
                //totalRecords++;
            }
            response.AssignedTasks = taskStatusList;
            response.TotalRecords = totalRecords;
            return response;
        }

        public int UpdateHours(int approvedApplicantId, int hours)
        {
            var result = _contributorRepository.UpdateHours(approvedApplicantId, hours);
            return result;
        }

        public TaskResponse GetAllTasks(string skill, int currentPageIndex, int recordsCount)
        {
            var user = _commonService.GetCurrentUserContext();
            var taskList = new TaskResponse();
            var totalRecords = 0;
            IEnumerable<task> dbTasks;
            IEnumerable<taskskill> dbTaskSkills;
            IEnumerable<Models.ResponseModels.Task.Task> tasks;
            if (string.IsNullOrEmpty(skill))
            {
                dbTasks = _contributorRepository.GetAllTasks(currentPageIndex, recordsCount, out totalRecords);
                tasks = _taskModelFactory.CreateModelList<Models.ResponseModels.Task.Task>(dbTasks);
            }
            else
            {
                dbTaskSkills = _contributorRepository.GetTasksBasedOnSkillOrDescription(skill, currentPageIndex, recordsCount, out totalRecords);
                tasks = _taskSkillModelFactory.CreateModelList<Models.ResponseModels.Task.Task>(dbTaskSkills);
            }
            taskList.Tasks = tasks;
            taskList.TotalRecords = totalRecords;
            var dbTaskApplicants = _contributorRepository.GetAppliedTasks(user.UserId);
            var dbApplicantSkills = _userRepository.GetSkillsByUserId(user.UserId);
            foreach (var appliedTask in dbTaskApplicants)
            {
                var taskObj = tasks.Where(x => x.Id == appliedTask.TASK_ID).FirstOrDefault();
                if (taskObj != null)
                {
                    taskObj.IsApplied = true;
                }
            }
            foreach (var dbskill in dbApplicantSkills)
            {
                foreach (var task in taskList.Tasks)
                {
                    if (task.Skills.Contains(dbskill.skill.VALUE))
                    {
                        task.IsRecommended = true;
                    }
                }
            }
            return taskList;
        }

        public TaskHistoryResponse GetTaskHistories(int currentPageIndex, int recordsCount)
        {
            var taskHistoryResponse = new TaskHistoryResponse();
            var tasksHistory = new List<TaskHistory>();
            var user = _commonService.GetCurrentUserContext();
            var totalRecords = 0;
            var dbapprovedapplicants = _contributorRepository.GetTaskHistories(user.UserId, currentPageIndex, recordsCount, out totalRecords);
            taskHistoryResponse.TotalRecords = totalRecords;
            foreach (var dbapprovedapplicant in dbapprovedapplicants)
            {
                var task = new TaskHistory();
                task.Task = _taskModelFactory.CreateModel<DCode.Models.ResponseModels.Task.Task>(dbapprovedapplicant.task);
                task.Applicant = _approvedApplicantModelFactory.CreateModel<ContributorSummary>(dbapprovedapplicant);
                tasksHistory.Add(task);
            }
            taskHistoryResponse.TaskHistories = tasksHistory;
            return taskHistoryResponse;
        }
    }
}

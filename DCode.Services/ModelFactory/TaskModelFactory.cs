using DCode.Common;
using DCode.Data.DbContexts;
using DCode.Models.RequestModels;
using DCode.Models.ResponseModels.Task;
using DCode.Services.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using static DCode.Models.Enums.Enums;

namespace DCode.Services.ModelFactory
{
    public class TaskModelFactory //: IModelFactory<task>
    {
        private CommonService _commonService;
        public TaskModelFactory(CommonService commonService)
        {
            _commonService = commonService;
        }

        public TModel CreateModel<TModel>(task input) where TModel : class
        {
            if (typeof(TModel) == typeof(TaskSummary))
            {
                return TranslateSummary(input) as TModel;
            }
            else
            {
                return TranslateTask(input) as TModel;
            }
        }

        private TaskSummary TranslateSummary(task input)
        {
            var taskSummary = new TaskSummary();
            if (input != null)
            {
                taskSummary.DueDate = CommonHelper.ConvertToDateUI(input.DUE_DATE);
                taskSummary.Hours = input.HOURS;
                taskSummary.Id = input.ID;
                taskSummary.ProjectName = input.PROJECT_NAME;
                taskSummary.TaskName = input.TASK_NAME;
            }
            return taskSummary;
        }

        private Models.ResponseModels.Task.Task TranslateTask(task input)
        {
            var task = new Models.ResponseModels.Task.Task();
            if (input != null)
            {
                task.Comments = input.COMMENTS;
                task.Details = input.DETAILS;
                task.DueDate = CommonHelper.ConvertToDateUI(input.DUE_DATE);
                if (input.user != null)
                {
                    task.FirstName = input.user.FIRST_NAME;
                    task.LastName = input.user.LAST_NAME;
                    task.RequestorEmailId = input.user.EMAIL_ID;

                }
                task.Hours = input.HOURS;
                task.Id = input.ID;
                task.ProjectName = input.PROJECT_NAME;
                task.ProjectWBSCode = input.PROJECT_WBS_Code;
                task.Skills = ConvertTaskSkillsToString(input.taskskills);
                task.Status = input.STATUS;
                task.StatusDate = input.STATUS_DATE;
                task.TypeId = input.TASK_TYPE_ID.GetValueOrDefault();
                task.OnBoardingDate = CommonHelper.ConvertToDateUI(input.ONBOARDING_DATE);
                task.ServiceLine = input.service_line?.Name;
                task.Duration = CommonHelper.CalculateDuration(input.CREATED_ON);
                task.TaskName = input.TASK_NAME;
            }
            return task;
        }

        private string ConvertTaskSkillsToString(ICollection<taskskill> collection)
        {
            var skills = string.Empty;
            foreach (var dbSkill in collection)
            {
                skills = skills + dbSkill.skill != null ? dbSkill.skill.VALUE : string.Empty;
            }
            return skills;
        }


        //public task CreateModel<TModel>(task input, TModel model) where TModel : class
        //{
        //    throw new NotImplementedException();
        //}

        public task CreateModel<TModel>(TModel input) where TModel : class
        {
            var dbTask = new task();
            if (typeof(TModel) == typeof(TaskRequest))
            {
                dbTask = TraslateTaskRequestToDbTask(input as TaskRequest);
            }
            else if (typeof(TModel) == typeof(Models.ResponseModels.Task.Task))
            {
                dbTask = TraslateTaskToDbTask(input as Models.ResponseModels.Task.Task);
            }

            return dbTask;
        }

        private task TraslateTaskRequestToDbTask(TaskRequest input)
        {
            var dbTask = new task();
            var user = _commonService.GetCurrentUserContext();
            var modelTask = input as TaskRequest;
            if (modelTask != null)
            {
                dbTask.ONBOARDING_DATE = Convert.ToDateTime(modelTask.OnBoardingDate, CultureInfo.InvariantCulture);
                dbTask.TASK_NAME = modelTask.TaskName;
                dbTask.COMMENTS = modelTask.Comments;
                dbTask.DETAILS = modelTask.Description;
                dbTask.DUE_DATE = Convert.ToDateTime(modelTask.DueDate, CultureInfo.InvariantCulture);
                dbTask.USER_ID = user.UserId;
                dbTask.HOURS = modelTask.Hours;
                dbTask.ID = modelTask.Id;
                dbTask.PROJECT_NAME = modelTask.ProjectName;
                dbTask.PROJECT_WBS_Code = modelTask.WBSCode;
                //dbTask.REQUESTOR_EMAIL_ID = user.EmailId;
                //dbTask.SKILLS = modelTask.SkillSet;
                dbTask.STATUS = TaskStatus.Active.ToString();
                dbTask.STATUS_DATE = DateTime.Now;
                dbTask.TASK_TYPE_ID = Convert.ToInt32(modelTask.SelectedTaskType);
                dbTask.SERVICE_LINE_ID = Convert.ToInt32(modelTask.SelectedServiceLine);
             
                if (Convert.ToDateTime(modelTask.DueDate) < DateTime.Today)
                {
                    input.Status = dbTask.STATUS = TaskStatus.Closed.ToString();
                }

            }
            return dbTask;
        }

        private task TraslateTaskToDbTask(Models.ResponseModels.Task.Task input)
        {
            var dbTask = new task();
            var modelTask = input as Models.ResponseModels.Task.Task;
            if (modelTask != null)
            {
                
                dbTask.COMMENTS = modelTask.Comments;
                //dbTask.CREATED_BY = modelTask.CreatedB
                dbTask.CREATED_ON = modelTask.CreatedOn;
                dbTask.DETAILS = modelTask.Details;
                dbTask.DUE_DATE = CommonHelper.ConvertToDateDatabase(modelTask.DueDate);
                dbTask.HOURS = modelTask.Hours;
                dbTask.ID = modelTask.Id;
                dbTask.PROJECT_NAME = modelTask.ProjectName;
                dbTask.PROJECT_WBS_Code = modelTask.ProjectWBSCode;
                //dbTask.REQUESTOR_EMAIL_ID = modelTask.RequestorEmailId;
                //dbTask.SKILLS = modelTask.Skills;
                dbTask.STATUS = modelTask.Status;
                dbTask.STATUS_DATE = modelTask.StatusDate;
                dbTask.TASK_TYPE_ID = modelTask.TypeId;
                //dbTask.UPDATED_BY = 
                //dbTask.UPDATED_ON =
                if (Convert.ToDateTime(modelTask.DueDate) < DateTime.Today)
                {
                    input.Status= dbTask.STATUS=TaskStatus.Closed.ToString();
                }
            }
            return dbTask;
        }

        public IEnumerable<TModel> CreateModelList<TModel>(IEnumerable<task> inputList) where TModel : class
        {
            var modelList = new List<Models.ResponseModels.Task.Task>();
            foreach (var dbTask in inputList)
            {
                var task = new Models.ResponseModels.Task.Task();
                task = CreateModel<Models.ResponseModels.Task.Task>(dbTask);
                modelList.Add(task);
            }
            return modelList as IEnumerable<TModel>;
        }
    }
}

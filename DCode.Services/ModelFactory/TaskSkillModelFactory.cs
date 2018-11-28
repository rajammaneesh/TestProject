using DCode.Common;
using DCode.Data.DbContexts;
using DCode.Services.ModelFactory.CommonFactory;
using System;
using System.Collections.Generic;
using static DCode.Models.Enums.Enums;

namespace DCode.Services.ModelFactory
{
    public class TaskSkillModelFactory : IModelFactory<taskskill>
    {

        public TModel CreateModel<TModel>(taskskill input) where TModel : class
        {
            if (typeof(TModel) == typeof(Models.ResponseModels.Task.Task))
            {
                return TranslateToTask(input) as TModel;
            }
            return null;
        }

        private Models.ResponseModels.Task.Task TranslateToTask(taskskill input)
        {
            var task = new Models.ResponseModels.Task.Task();
            task.Comments = input.task.COMMENTS;
            task.CreatedBy = input.task.CREATED_BY;
            task.CreatedOn = input.task.CREATED_ON;
            task.Details = input.task.DETAILS;
            task.DueDate = CommonHelper.ConvertToDateUI(input.task.DUE_DATE);
            task.Duration = CommonHelper.CalculateDuration(input.task.CREATED_ON);
            task.FirstName = input.task.user.FIRST_NAME;
            task.LastName = input.task.user.LAST_NAME;
            task.OnBoardingDate = CommonHelper.ConvertToDateUI(input.task.ONBOARDING_DATE);
            task.ProjectName = input.task.PROJECT_NAME;
            task.ProjectWBSCode = input.task.PROJECT_WBS_Code;
            task.RequestorEmailId = input.task.user.EMAIL_ID;
            task.Skills = input.skill.VALUE;
            task.Status = input.task.STATUS;
            task.StatusDate = input.task.STATUS_DATE;
            task.TaskName = input.task.TASK_NAME;
            task.TypeId = input.task.TASK_TYPE_ID.GetValueOrDefault();
            task.UpdatedBy = input.task.UPDATED_BY;
            task.UpdatedOn = input.task.UPDATED_ON;
            task.Id = input.task.ID;
            task.Hours = input.task.HOURS;
            task.Offering = input.task.offering.Code;
            task.OfferingId = Convert.ToString(input.task.offering.Id);
            task.SubOfferingId = Convert.ToString(input.task.SUB_OFFERING_ID);

            return task;
        }

        public taskskill CreateModel<TModel>(taskskill input, TModel model) where TModel : class
        {
            throw new NotImplementedException();
        }

        public taskskill CreateModel<TModel>(TModel input) where TModel : class
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TModel> CreateModelList<TModel>(IEnumerable<taskskill> inputList) where TModel : class
        {
            if (typeof(TModel) == typeof(Models.ResponseModels.Task.Task))
            {
                return TranslateToTaskList(inputList) as IEnumerable<TModel>;
            }
            return null;
        }

        private IEnumerable<Models.ResponseModels.Task.Task> TranslateToTaskList(IEnumerable<taskskill> inputList)
        {
            var taskList = new List<Models.ResponseModels.Task.Task>();
            foreach (var task in inputList)
            {
                var dbtask = CreateModel<Models.ResponseModels.Task.Task>(task);
                taskList.Add(dbtask);
            }
            return taskList as IEnumerable<Models.ResponseModels.Task.Task>;
        }

        public IEnumerable<taskskill> CreateModelList(IEnumerable<int> skillIdList)
        {
            var dbTaskSkills = new List<taskskill>();
            if (skillIdList != null)
            {
                foreach (var skillId in skillIdList)
                {
                    var dbTaskSkill = new taskskill();
                    dbTaskSkill.SKILL_ID = skillId;
                    dbTaskSkill.STATUS = SkillStatus.Active.ToString();
                    dbTaskSkill.STATUS_DATE = DateTime.Now;
                    dbTaskSkills.Add(dbTaskSkill);
                }
            }
            return dbTaskSkills;
        }
    }
}

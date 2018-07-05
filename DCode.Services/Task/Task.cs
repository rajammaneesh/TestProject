﻿using DCode.Common;
using DCode.Data.DbContexts;
using DCode.Data.TaskRepository;
using DCode.Models.RequestModels;
using DCode.Models.ResponseModels.Common;
using DCode.Services.Base;
using DCode.Services.Common;
using DCode.Services.ModelFactory;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using static DCode.Models.Enums.Enums;

namespace DCode.Services.Task
{
    public class Task : BaseService, ITask
    {
        private ITaskRepository _taskRepository;
        private TaskModelFactory _taskModelFactory;
        private TaskSkillModelFactory _taskSkillModelFactory;
        private SkillModelFactory _skillModelFactory;

        private ICommonService _commonService;

        public Task(ITaskRepository taskRepository, TaskModelFactory taskModelFactory, TaskSkillModelFactory taskSkillModelFactory, SkillModelFactory skillModelFactory, ICommonService commonService)
        {
            _taskRepository = taskRepository;
            _taskModelFactory = taskModelFactory;
            _taskSkillModelFactory = taskSkillModelFactory;
            _skillModelFactory = skillModelFactory;
            _commonService = commonService;
        }

        public int UpsertTask(TaskRequest taskRequest)
        {
            var result = 0;
            if (taskRequest.ActionType == ActionType.Insert)
            {
                taskRequest.WBSCode = string.IsNullOrWhiteSpace(taskRequest.WBSCode)
                    ? "WBS00000-00-00-00-0000"
                    : taskRequest.WBSCode;

                var dbTask = _taskModelFactory.CreateModel<TaskRequest>(taskRequest);
                MapAuditFields<task>(ActionType.Insert, dbTask);
                if (taskRequest.SelectedTaskType == "2")
                {

                    if (taskRequest.SkillSet == null
                        || !taskRequest.SkillSet.Any())
                    {
                        var listOfSkills = new List<int>();

                        var firmInitiativeSkill = _taskRepository.GetSkillByName(Constants.FirmInitiativeSkillRecord);

                        listOfSkills.Add(firmInitiativeSkill != null
                            ? firmInitiativeSkill.ID
                            : default(int));

                        taskRequest.SkillSet = listOfSkills;
                    }
                }
                var dbTaskSkills = _taskSkillModelFactory.CreateModelList(taskRequest.SkillSet);
                foreach (var dbTaskSkill in dbTaskSkills)
                {
                    MapAuditFields<taskskill>(ActionType.Insert, dbTaskSkill);
                }
                result = _taskRepository.InsertTask(dbTask, dbTaskSkills);

                if (taskRequest.SelectedTaskType == "2"
                    && result == 2)
                {
                    var currentUser = _commonService.GetCurrentUserContext();

                    EmailHelper.PostNewFINotification(taskRequest.ProjectName,
                        taskRequest.Hours.ToString(),
                        taskRequest.Description,
                        taskRequest.OnBoardingDate,
                        currentUser.EmailId,
                        GetConsultingEmailUsers());
                }
            }
            else if (taskRequest.ActionType == ActionType.Update)
            {
                var dbTask = _taskModelFactory.CreateModel<TaskRequest>(taskRequest);
                MapAuditFields<task>(ActionType.Update, dbTask);
                result = _taskRepository.UpdateTask(dbTask);
            }
            return result;
        }

        public int CloseTask(int taskId)
        {
            return _taskRepository.CloseTask(taskId);
        }

        public IEnumerable<Skill> GetSkills()
        {
            var dbSkills = _taskRepository.GetAllSkills();
            return _skillModelFactory.CreateModelList<Skill>(dbSkills);
        }

        public IEnumerable<DCode.Models.ResponseModels.Task.Task> GetTasks()
        {
            var dbTasks = _taskRepository.GetTasks();
            var result = _taskModelFactory.CreateModelList<DCode.Models.ResponseModels.Task.Task>(dbTasks) as IEnumerable<DCode.Models.ResponseModels.Task.Task>;
            return result;
        }

        private List<string> GetConsultingEmailUsers()
        {
            var userEmails = ConfigurationManager.AppSettings["USIConsultingAddresses"];

            var userEmailList = userEmails?.Split(';')?.ToList();

            userEmailList?.RemoveAll(x => string.IsNullOrWhiteSpace(x));

            return userEmailList;
        }
    }
}

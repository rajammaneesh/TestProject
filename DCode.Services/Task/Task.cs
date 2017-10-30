﻿using DCode.Data.DbContexts;
using DCode.Data.TaskRepository;
using DCode.Models.RequestModels;
using DCode.Models.ResponseModels.Common;
using DCode.Services.Base;
using DCode.Services.ModelFactory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCode.Services.Task
{
    public class Task : BaseService, ITask
    {
        private ITaskRepository _taskRepository;
        private TaskModelFactory _taskModelFactory;
        private TaskSkillModelFactory _taskSkillModelFactory;
        private SkillModelFactory _skillModelFactory;
        public Task(ITaskRepository taskRepository, TaskModelFactory taskModelFactory, TaskSkillModelFactory taskSkillModelFactory, SkillModelFactory skillModelFactory)
        {
            _taskRepository = taskRepository;
            _taskModelFactory = taskModelFactory;
            _taskSkillModelFactory = taskSkillModelFactory;
            _skillModelFactory = skillModelFactory;
        }

        public int UpsertTask(TaskRequest taskRequest)
        {
            var result = 0;
            if (taskRequest.ActionType == DCode.Common.Enums.ActionType.Insert)
            {
                var dbTask = _taskModelFactory.CreateModel<TaskRequest>(taskRequest);
                MapAuditFields<task>(DCode.Common.Enums.ActionType.Insert, dbTask);
                var dbTaskSkills = _taskSkillModelFactory.CreateModelList(taskRequest.SkillSet);
                foreach (var dbTaskSkill in dbTaskSkills)
                {
                    MapAuditFields<taskskill>(DCode.Common.Enums.ActionType.Insert, dbTaskSkill);
                }
                result = _taskRepository.InsertTask(dbTask, dbTaskSkills);
            }
            else if (taskRequest.ActionType == DCode.Common.Enums.ActionType.Update)
            {
                var dbTask = _taskModelFactory.CreateModel<TaskRequest>(taskRequest);
                MapAuditFields<task>(DCode.Common.Enums.ActionType.Update, dbTask);
                result = _taskRepository.UpdateTask(dbTask);
            }
            return result;
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
    }
}

﻿using DCode.Common;
using DCode.Data.DbContexts;
using DCode.Data.TaskRepository;
using DCode.Models.Email;
using DCode.Models.RequestModels;
using DCode.Models.ResponseModels.Common;
using DCode.Services.Base;
using DCode.Services.Common;
using DCode.Services.Email;
using DCode.Services.ModelFactory;
using System;
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
        private TaskSubOfferingMapModelFactory _taskSubOfferingMapModelFactory;

        private ICommonService _commonService;
        private IEmailTrackerService _emailTrackerService;
        private IODCService _odcService;

        public Task(ITaskRepository taskRepository, TaskModelFactory taskModelFactory, TaskSkillModelFactory taskSkillModelFactory, SkillModelFactory skillModelFactory, ICommonService commonService, IEmailTrackerService emailTrackerService, TaskSubOfferingMapModelFactory taskSubOfferingMapModelFactory, IODCService odcService)
        {
            _taskRepository = taskRepository;
            _taskModelFactory = taskModelFactory;
            _taskSkillModelFactory = taskSkillModelFactory;
            _skillModelFactory = skillModelFactory;
            _commonService = commonService;
            _emailTrackerService = emailTrackerService;
            _taskSubOfferingMapModelFactory = taskSubOfferingMapModelFactory;
            _odcService = odcService;
        }

        public int UpsertTask(TaskRequest taskRequest)
        {
            var result = 0;
            var isODCTask = false;
            if (taskRequest.ActionType == ActionType.Insert)
            {
                taskRequest.WBSCode = string.IsNullOrWhiteSpace(taskRequest.WBSCode)
                    ? "WBS00000-00-00-00-0000"
                    : taskRequest.WBSCode;

                var dbTask = _taskModelFactory.CreateModel<TaskRequest>(taskRequest);
                MapAuditFields<task>(ActionType.Insert, dbTask);
                if (taskRequest.SelectedTaskType == "2"
                    || taskRequest.SelectedTaskType == "3")
                {

                    if (taskRequest.SkillSet == null
                        || !taskRequest.SkillSet.Any())
                    {
                        var listOfSkills = new List<int>();

                        var skillName = taskRequest.SelectedTaskType == "2"
                            ? Constants.FirmInitiativeSkillRecord
                            : Constants.IndustryInitiativeSkillRecord;

                        var matchedSkill = _taskRepository.GetSkillByName(skillName);

                        listOfSkills.Add(matchedSkill != null
                            ? matchedSkill.ID
                            : default(int));

                        taskRequest.SkillSet = listOfSkills;
                    }
                }
                var dbTaskSkills = _taskSkillModelFactory.CreateModelList(taskRequest.SkillSet);
                foreach (var dbTaskSkill in dbTaskSkills)
                {
                    MapAuditFields<taskskill>(ActionType.Insert, dbTaskSkill);
                }

                //Get the model for Task_suboffering map
                var subOfferingMap = _taskSubOfferingMapModelFactory.CreateModel<task_suboffering_map>(taskRequest.SelectedSubOffering);
                if (subOfferingMap != null)
                {
                    MapAuditFields<task_suboffering_map>(ActionType.Insert, subOfferingMap);
                }

                result = _taskRepository.InsertTask(dbTask, dbTaskSkills, subOfferingMap);

                if (taskRequest.SelectedTaskType == "2"
                    && result == 2)
                {
                    var currentUser = _commonService.GetCurrentUserContext();
                    var offeringRecipients = new List<string>();

                    var odcMailFooterText = string.Empty;
                    var odcMailHeaderImage = string.Empty;

                    //If  the task is created for a sub offering, then fethc the maill list for the selected sub offering id
                    if (!string.IsNullOrEmpty(taskRequest.SelectedSubOffering) && Convert.ToInt32(taskRequest.SelectedSubOffering) > 0)
                    {
                        offeringRecipients = _commonService.GetFINotificationRecipientsForSubOffering(
                        Convert.ToInt32(taskRequest.SelectedSubOffering));
                        isODCTask = true;
                        var odc = _odcService.GetExistingODCByOfferingId(AppDomain.CurrentDomain.BaseDirectory + Constants.ODCPath, taskRequest.SelectedOffering);
                        if (odc != null)
                        {
                            odcMailFooterText = odc.MailFooterText;
                            odcMailHeaderImage = odc.MailHeaderImage;
                        }
                    }
                    else
                    {
                        offeringRecipients = _commonService.GetFINotificationRecipientsForOffering(
                           Convert.ToInt32(taskRequest.SelectedOffering));
                    }
                    offeringRecipients = offeringRecipients != null && offeringRecipients.Any()
                        ? offeringRecipients
                        : _commonService.GetDefaultConsultingMailboxes();

                    var offering = _commonService.GetOfferings()
                        .Where(x => x.Id == Convert.ToInt32(taskRequest.SelectedOffering))
                        .Select(x => x.Description)
                        .FirstOrDefault();

                    var mailMessage = EmailHelper.PostNewFINotification(taskRequest.ProjectName,
                        taskRequest.Hours.ToString(),
                        taskRequest.Description,
                        taskRequest.OnBoardingDate,
                        currentUser.EmailId,
                       offeringRecipients,
                       offering, odcMailFooterText, odcMailHeaderImage);
                    var emailTracker = new EmailTracker
                    {
                        ToAddresses = ConfigurationManager.AppSettings["DcodeEmailId"],
                        Subject = mailMessage.Subject,
                        Body = mailMessage.Body,
                        TaskId = taskRequest.Id,
                        Source = ApplicationSource.WebApp.ToString()
                    };

                    if (currentUser.EmailId != null)
                    {
                        emailTracker.CcAddresses.Add(currentUser.EmailId);
                    }


                    if (offeringRecipients != null && offeringRecipients.Any())
                    {
                        emailTracker.BccAddresses.AddRange(offeringRecipients);
                    }

                    _emailTrackerService.InsertEmail(emailTracker);
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

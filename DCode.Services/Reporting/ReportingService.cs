using DCode.Data.TaskRepository;
using DCode.Data.UserRepository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DCode.Services.Reporting
{
    public class ReportingService : IReportingService
    {
        private readonly ITaskRepository _taskRepository;

        private readonly IUserRepository _userRepository;

        public ReportingService(ITaskRepository taskRepository,
            IUserRepository userRepository)
        {
            _taskRepository = taskRepository;

            _userRepository = userRepository;
        }

        public IEnumerable<string> GetSubscribedUserForTask(string task)
        {
            return _userRepository
                 .GetSubscribedUserForTask(task);
        }

        public IEnumerable<string> GetSkillsForNewTasksAddeddYesterday()
        {
            return _taskRepository
                .GetTaskCountBySkillForDate(DateTime.Now.AddDays(-1))
                ?.SkipWhile(x => x.Item2 == 0)
                ?.Select(x => x.Item1)
                .Distinct();
        }

        public IEnumerable<Tuple<string, string>> GetProjectDetailsForNewTasksAddedYesterday(string skill)
        {
            var projectDetails = _taskRepository
                  .GetProjectDetailsForNewTasksFromDateForSkill(DateTime.Now.AddDays(-1), skill);

            var result = new List<Tuple<string, string>>();

            foreach (var project in projectDetails)
            {
                result.Add(
                    Tuple.Create<string, string>(project.PROJECT_NAME, project.TASK_NAME));
            }

            return result;
        }
    }
}

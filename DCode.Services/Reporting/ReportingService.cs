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
    }
}

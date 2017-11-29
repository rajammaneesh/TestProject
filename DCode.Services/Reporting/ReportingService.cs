using DCode.Data.ReportingRepository;
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

        private readonly IDailyUsageStatisticsRepository _dailyUsageStatisticsRepository;

        public ReportingService(ITaskRepository taskRepository,
            IUserRepository userRepository,
            IDailyUsageStatisticsRepository dailyUsageStatisticsRepository)
        {
            _taskRepository = taskRepository;

            _userRepository = userRepository;

            _dailyUsageStatisticsRepository = dailyUsageStatisticsRepository;
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

        public List<Tuple<DateTime, int>> GetUserVisitsCount(int noOfRecords = 1)
        {
            List<Tuple<DateTime, int>> recordsCount = null;

            for (int i = 0; i < noOfRecords; i++)
            {
                var dateTime = DateTime.Now.AddDays(-1 * i);

                var result = _dailyUsageStatisticsRepository.GetDailyStatisticsFor(dateTime);

                recordsCount = recordsCount ?? new List<Tuple<DateTime, int>>();

                recordsCount.Add(
                    Tuple.Create(
                        dateTime,
                        result != null && result.Any()
                            ? Convert.ToInt32(result.First().visits)
                            : 0));
            }

            return recordsCount;
        }

        public void UpdateDailySiteVisitCount()
        {
            _dailyUsageStatisticsRepository.UpsertDailyStatistics();
        }
    }
}

using System;
using System.Collections.Generic;

namespace DCode.Services.Reporting
{
    public interface IReportingService
    {
        IEnumerable<string> GetSubscribedUserForTask(string task);

        IEnumerable<string> GetSkillsForNewTasksAddeddYesterday();
    }
}

using DCode.Models.RequestModels;
using DCode.Models.ResponseModels.Common;
using System.Collections.Generic;


namespace DCode.Services.Task
{
    public interface ITask
    {
        int UpsertTask(TaskRequest taskRequest);
        IEnumerable<Skill> GetSkills();
        IEnumerable<DCode.Models.ResponseModels.Task.Task> GetTasks();
        int CloseTask(int taskId);
    }
}

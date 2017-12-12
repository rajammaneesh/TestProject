using System.Collections.Generic;

namespace DCode.Models.ResponseModels.Requestor
{
    public class TaskStatusResponse
    {
        public IEnumerable<TaskStatus> TaskStatuses { get; set; }
        public int TotalRecords { get; set; }
    }
}

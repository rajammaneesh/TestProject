using System.Collections.Generic;

namespace DCode.Models.ResponseModels.Task
{
    public class TaskResponse
    {
        public IEnumerable<Task> Tasks { get; set; }
        public int TotalRecords { get; set; }
    }
}

using System.Collections.Generic;

namespace DCode.Models.ResponseModels.Contributor
{
    public class AssignedTasksResponse
    {
        public IEnumerable<AssignedTask> AssignedTasks { get; set; }
        public int TotalRecords { get; set; }
    }
}

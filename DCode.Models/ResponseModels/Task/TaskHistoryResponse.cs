using DCode.Models.ResponseModels.Requestor;
using System.Collections.Generic;

namespace DCode.Models.ResponseModels.Task
{
    public class TaskHistoryResponse
    {
        public List<TaskHistory> TaskHistories { get; set; }
        public int TotalRecords { get; set; }
    }
}

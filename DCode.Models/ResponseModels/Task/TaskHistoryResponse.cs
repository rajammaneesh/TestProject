using DCode.Models.ResponseModels.Requestor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCode.Models.ResponseModels.Task
{
    public class TaskHistoryResponse
    {
        public List<TaskHistory> TaskHistories { get; set; }
        public int TotalRecords { get; set; }
    }
}

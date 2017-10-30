using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCode.Models.ResponseModels.Requestor
{
    public class TaskStatusResponse
    {
        public IEnumerable<TaskStatus> TaskStatuses { get; set; }
        public int TotalRecords { get; set; }
    }
}

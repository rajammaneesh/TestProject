using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCode.Models.ResponseModels.Task
{
    public class TaskResponse
    {
        public IEnumerable<Task> Tasks { get; set; }
        public int TotalRecords { get; set; }
    }
}

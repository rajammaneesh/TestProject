using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCode.Models.ResponseModels.Contributor
{
    public class AssignedTasksResponse
    {
        public IEnumerable<AssignedTask> AssignedTasks { get; set; }
        public int TotalRecords { get; set; }
    }
}

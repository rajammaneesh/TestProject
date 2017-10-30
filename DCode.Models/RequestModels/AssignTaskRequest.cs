using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCode.Models.ResponseModels.Requestor
{
    public class AssignTaskRequest
    {
        public int TaskId { get; set; }
        public int ApplicantId { get; set; }
        public int TaskApplicantId { get; set; }
    }
}

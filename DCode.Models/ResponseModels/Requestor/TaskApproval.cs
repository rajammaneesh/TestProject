using DCode.Models.ResponseModels.Contributor;
using DCode.Models.ResponseModels.Task;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCode.Models.ResponseModels.Requestor
{
    public class TaskApproval
    {
        public int TaskApplicantId { get; set; }
        public Models.ResponseModels.Task.Task Task { get; set; }
        public ICollection<Models.ResponseModels.Contributor.Contributor> Applicants { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCode.Models.ResponseModels.Requestor
{
    public class TaskHistory
    {
        public Task.Task Task { get; set; }
        public Contributor.ContributorSummary Applicant { get; set; }
    }
}

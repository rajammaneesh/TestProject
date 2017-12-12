using System.Collections.Generic;

namespace DCode.Models.ResponseModels.Requestor
{
    public class TaskApproval
    {
        public int TaskApplicantId { get; set; }
        public Models.ResponseModels.Task.Task Task { get; set; }
        public ICollection<Models.ResponseModels.Contributor.Contributor> Applicants { get; set; }
    }
}

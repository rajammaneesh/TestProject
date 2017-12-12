using System.Collections.Generic;

namespace DCode.Models.ResponseModels.Requestor
{
    public class TaskApplicantsReponse
    {
        public IEnumerable<TaskApproval> TaskApprovals { get; set; }
        public int TotalRecords { get; set; }
    }
}

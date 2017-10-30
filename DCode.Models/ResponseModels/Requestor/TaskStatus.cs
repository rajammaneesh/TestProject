using DCode.Common;
using DCode.Models.ResponseModels.Contributor;
using DCode.Models.ResponseModels.Task;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCode.Models.ResponseModels.Requestor
{
    public class TaskStatus
    {
        public int ApprovedApplicantId { get; set; }
        public int TaskApplicantId { get; set; }
        //public RequestorSummary Requestor { get; set; }
        public Contributor.Contributor Applicant { get; set; }
        public DCode.Models.ResponseModels.Task.Task Task { get; set; }
        //Needed for progress bar under trackstatus tab
        public string ProgressWidth { get { return (Applicant.CompletedHours.HasValue && Task.Hours.HasValue && Task.Hours.Value > 0) ? ((Applicant.CompletedHours / Task.Hours) * 100).ToString() : Constants.Zero ;} }
        public bool? IsWorkAgainClicked { set{value = null;} }
        public bool? IsRatingClicked { set { value = null; } }
        public bool? EnableReviewApplicant { set { value = null; } }
        public string Duration { get; set; }
        public string Comments { get; set; }
    }
}

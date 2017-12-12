namespace DCode.Models.ResponseModels.Contributor
{
    public class AssignedTask
    {
        public int ApprovedApplicantId { get; set; }
        public int TaskApplicantId { get; set; }
        public Contributor Applicant { get; set; }
        public DCode.Models.ResponseModels.Task.Task Task { get; set; }
        //Needed for progress bar under trackstatus tab
        public string ProgressWidth { get { return (Applicant.CompletedHours.HasValue && Task.Hours.HasValue && Task.Hours.Value > 0) ? ((Applicant.CompletedHours / Task.Hours) * 100).ToString() : "0"; } }

        public bool? EnableReviewApplicant { set { value = null; } }

    }
}

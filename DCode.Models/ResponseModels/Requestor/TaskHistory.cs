namespace DCode.Models.ResponseModels.Requestor
{
    public class TaskHistory
    {
        public Task.Task Task { get; set; }
        public Contributor.ContributorSummary Applicant { get; set; }
    }
}

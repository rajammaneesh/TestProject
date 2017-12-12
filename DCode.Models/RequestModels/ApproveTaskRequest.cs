namespace DCode.Models.RequestModels
{
    public class ApproveTaskRequest
    {
        public int TaskApplicantId { get; set; }
        public int TaskId { get; set; }
        public int ApplicantId { get; set; }
    }
}

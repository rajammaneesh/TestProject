namespace DCode.Models.RequestModels
{
    public class RejectTaskRequest
    {
        public int TaskId { get; set; }
        public int ApplicantId { get; set; }
        public int TaskApplicantId { get; set; }
    }
}

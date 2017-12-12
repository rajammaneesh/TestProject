namespace DCode.Models.ResponseModels.Requestor
{
    public class AssignTaskRequest
    {
        public int TaskId { get; set; }
        public int ApplicantId { get; set; }
        public int TaskApplicantId { get; set; }
    }
}

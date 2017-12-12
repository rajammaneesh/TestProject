namespace DCode.Models.RequestModels
{
    public class ApplicantRequest
    {
        public int TaskId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailId { get; set; }
        public string ManagerEmailId { get; set; }
    }
}

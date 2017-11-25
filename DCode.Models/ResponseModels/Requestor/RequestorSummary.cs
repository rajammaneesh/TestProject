namespace DCode.Models.ResponseModels.Requestor
{
    public class RequestorSummary
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Name
        {
            get { return $"{FirstName},{LastName}"; }
        }
        public string EmailId { get; set; }
        //Do not add more properties, use instead Requestor model
    }
}

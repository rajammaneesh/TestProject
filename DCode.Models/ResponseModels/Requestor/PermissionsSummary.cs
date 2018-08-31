namespace DCode.Models.ResponseModels.Requestor
{
    public class PermissionsSummary
    {
        public int ApplicantId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Name
        {
            get;
            set;
        }
        public string CircledName
        {
            get;
            set;
        }
        public string Status
        {
            get;set;
        }

        public string Designation
        {
            get; set;
        }

        public string Skills
        {
            get; set;
        }

        public string Days
        {
            get; set;
        }
    }
}

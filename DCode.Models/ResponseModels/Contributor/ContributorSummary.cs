using System;
using DCode.Models.ResponseModels.Base;

namespace DCode.Models.ResponseModels.Contributor
{
    public class ContributorSummary : ResponseBase
    {
        public int ApplicantId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Name
        {
            get { return (String.IsNullOrEmpty(FirstName) && String.IsNullOrEmpty(LastName)) ? string.Empty : $"{FirstName} {LastName}"; }
        }
        public string EmailId { get; set; }
        public int TopRatingsCount { get; set; }
        public string ShortName
        {
            get
            {
                if (!String.IsNullOrEmpty(Name) && Name.Trim().Contains(" "))
                {
                    var split = Name.Split(' ');
                    return split[0].Substring(0, 1) + string.Empty + split[1].Substring(0, 1);
                }
                else
                {
                    return String.IsNullOrEmpty(Name) ? string.Empty : Name.Substring(0, 2);
                }
            }
        }
        //Do not add more properties, use instead Contributor model
    }
}

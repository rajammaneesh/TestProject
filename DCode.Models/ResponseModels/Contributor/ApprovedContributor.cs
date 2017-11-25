using System;
using static DCode.Models.Enums.Enums;

namespace DCode.Models.ResponseModels.Contributor
{
    public class ApprovedContributor
    {
        public int Id { get; set; }
        public int? ApplicantId { get; set; }
        public ApplicantStatus ApplicantStatus { get; set; }
        public string Rating { get; set; }
        public bool? WorkAgain { get; set; }
        public int? Points { get; set; }
        public decimal? HoursWorked { get; set; }
        public string Comments { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
    }
}

using DCode.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCode.Models.ResponseModels.Contributor
{
    public class ApprovedContributor
    {
        public int Id { get; set; }
        public int? ApplicantId { get; set; }
        public Enums.ApplicantStatus ApplicantStatus { get; set; }
        public string Rating { get; set; }
        public bool? WorkAgain { get; set; }
        public int? Points { get; set; }
        public decimal? HoursWorked { get; set; }
        public string Comments { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
    }
}

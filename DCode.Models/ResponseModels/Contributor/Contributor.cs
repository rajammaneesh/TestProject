using DCode.Models.Base;
using DCode.Models.ResponseModels.Base;
using DCode.Models.ResponseModels.Contributor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCode.Models.ResponseModels.Contributor
{
    public class Contributor : ContributorSummary, IViewModel
    {
        public string Designation { get; set; }
        public string Expertise { get; set; }
        public decimal? CompletedHours { get; set; }
        public string Duration { get; set; }
        public int TaskApplicantId { get; set; }

        public string StatementOfPurpose { get; set; }

        public string CreatedBy
        {
            get;
            set;
        }

        public DateTime? CreatedOn
        {
            get;
            set;
        }

        public string UpdatedBy
        {
            get;
            set;
        }

        public DateTime? UpdatedOn
        {
            get;
            set;
        }

        public string Status
        {
            get;
            set;
        }

        public DateTime? StatusDate
        {
            get;
            set;
        }

        public ICollection<Models.ResponseModels.Common.ManagerComments> Comments
        {
            get;
            set;
        }
    }
}

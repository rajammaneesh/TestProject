using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCode.Models.ResponseModels.Common
{
   public class ApprovedApplicant
    {
        public int ID { get; set; }
        public int APPLICANT_ID { get; set; }
        public int TASK_ID { get; set; }
        public Nullable<decimal> HOURS_WORKED { get; set; }
        public string RATING { get; set; }
        public Nullable<bool> WORK_AGAIN { get; set; }
        public Nullable<int> POINTS { get; set; }
        public string COMMENTS { get; set; }
        public string STATUS { get; set; }
        public Nullable<System.DateTime> STATUS_DATE { get; set; }
        public string CREATED_BY { get; set; }
        public Nullable<System.DateTime> CREATED_ON { get; set; }
        public string UPDATED_BY { get; set; }
        public Nullable<System.DateTime> UPDATED_ON { get; set; }
    }
}

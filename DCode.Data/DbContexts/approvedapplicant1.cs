//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DCode.Data.DbContexts
{
    using System;
    using System.Collections.Generic;
    
    public partial class approvedapplicant1
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
    
        public virtual user1 user { get; set; }
        public virtual task1 task { get; set; }
    }
}

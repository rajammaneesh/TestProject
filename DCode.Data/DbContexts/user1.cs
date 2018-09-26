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
    
    public partial class user1
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public user1()
        {
            this.approvedapplicants = new HashSet<approvedapplicant1>();
            this.notification_subscription = new HashSet<notification_subscription1>();
            this.taskapplicants = new HashSet<taskapplicant1>();
            this.tasks = new HashSet<task1>();
            this.user_points = new HashSet<user_points1>();
        }
    
        public int ID { get; set; }
        public string FIRST_NAME { get; set; }
        public string LAST_NAME { get; set; }
        public string DESIGNATION { get; set; }
        public string EMAIL_ID { get; set; }
        public Nullable<int> location_id { get; set; }
        public string PROJECT_NAME { get; set; }
        public string PROJECT_CODE { get; set; }
        public string PROJECT_MANAGER_NAME { get; set; }
        public string MANAGER_EMAIL_ID { get; set; }
        public string STATUS { get; set; }
        public Nullable<System.DateTime> STATUS_DATE { get; set; }
        public string CREATED_BY { get; set; }
        public Nullable<System.DateTime> CREATED_ON { get; set; }
        public string UPDATED_BY { get; set; }
        public Nullable<System.DateTime> UPDATED_ON { get; set; }
        public Nullable<int> OFFERING_ID { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<approvedapplicant1> approvedapplicants { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<notification_subscription1> notification_subscription { get; set; }
        public virtual offering1 offering { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<taskapplicant1> taskapplicants { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<task1> tasks { get; set; }
        public virtual user_locations user_locations { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<user_points1> user_points { get; set; }
    }
}
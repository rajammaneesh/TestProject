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
    
    public partial class offering
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public offering()
        {
            this.users = new HashSet<user>();
            this.tasks = new HashSet<task>();
        }
    
        public int Id { get; set; }
        public string Description { get; set; }
        public int Portfolio_Id { get; set; }
        public string Code { get; set; }
        public string RM_Email_Group { get; set; }
        public string Practice_Email_Group { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<user> users { get; set; }
        public virtual portfolio portfolio { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<task> tasks { get; set; }
    }
}

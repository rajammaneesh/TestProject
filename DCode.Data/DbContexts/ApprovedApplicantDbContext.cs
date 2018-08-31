using DCode.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace DCode.Data.DbContexts
{
   public class ApprovedApplicantDbContext: DbContext
    {
        public ApprovedApplicantDbContext()
            : base(Constants.DCodeEntities)
        {
            this.Configuration.LazyLoadingEnabled = false;
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
        public virtual DbSet<approvedapplicant> Approvedapplicants { get; set; }
    }
}

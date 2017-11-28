using DCode.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace DCode.Data.DbContexts
{
    public partial class TaskDbContext : DbContext
    {
        public TaskDbContext() : base(Constants.DCodeEntities)
        {
            this.Configuration.LazyLoadingEnabled = false;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }

        public virtual DbSet<task> Tasks{get;set;}
        public virtual DbSet<approvedapplicant> ApprovedApplicants { get; set; }
        public virtual DbSet<taskapplicant> TaskApplicants{get;set;}
        public virtual DbSet<taskskill> TaskSkills{get;set;}
    }
}

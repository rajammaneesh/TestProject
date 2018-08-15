using DCode.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace DCode.Data.DbContexts
{
    public partial class EmailDbContext : DbContext
    {
        public EmailDbContext()
            : base(Constants.DCodeEntities)
        {
            this.Configuration.LazyLoadingEnabled = false;
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
        public virtual DbSet<emailtracker> EmailTracker { get; set; }
    }
}

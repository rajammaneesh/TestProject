using DCode.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace DCode.Data.DbContexts
{
    public class MetadataDbContext : DbContext
    {
        public MetadataDbContext()
            : base(Constants.DCodeEntities)
        {
            this.Configuration.LazyLoadingEnabled = false;
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
        public virtual DbSet<service_line> ServiceLines { get; set; }

        public virtual DbSet<offering> Offerings { get; set; }

        public virtual DbSet<portfolio> Portfolios { get; set; }
    }
}

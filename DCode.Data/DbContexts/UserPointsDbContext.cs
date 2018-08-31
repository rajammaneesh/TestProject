using DCode.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace DCode.Data.DbContexts
{
   public class UserPointsDbContext:DbContext
    {
        public UserPointsDbContext()
            : base(Constants.DCodeEntities)
        {
            this.Configuration.LazyLoadingEnabled = false;
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
        public virtual DbSet<user_points> UserPoints { get; set; }
    }
}

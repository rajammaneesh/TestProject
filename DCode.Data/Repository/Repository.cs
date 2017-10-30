using DCode.Common;
using DCode.Data.DbContexts;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCode.Data.Repository
{
    public abstract class Repository<TEntity> : IRepository<TEntity>, IDisposable
        where TEntity : class
    {
        /// <summary>
        /// Creates a new Repository.
        /// </summary>
        protected Repository() { }
        private bool _disposed;

        /// <summary>
        /// Creates a new Repository.
        /// </summary>
        /// <param name="context">Entity Framework DbContext-derived class.</param>
        protected Repository(DbContext context)
        {
            if (context != null)
            {
                Context = context;
                Context.Configuration.LazyLoadingEnabled = false;
                Context.Configuration.AutoDetectChangesEnabled = true;
                Context.Configuration.ProxyCreationEnabled = false;
                Context.Configuration.ValidateOnSaveEnabled = true;
            }
        }

        /// <summary>
        /// Gets and sets the DbContext for the repository.
        /// </summary>
        protected DbContext Context { get; set; }

        /// <summary>
        /// Gets the DbSet for the respository
        /// </summary>
        protected DbSet<TEntity> Set
        {
            get
            {
                return Context.Set<TEntity>();
            }
        }

        public DbSet<TEntity> DbSet
        {
            get { return Set; }
        }

        /// <summary>
        /// Disposes the DbContext.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes the DbContext.
        /// </summary>
        /// <param name="disposing">True to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }
            if (disposing)
            {
                Context.Dispose();
            }
            _disposed = true;
        }



        public TEntity Find(params object[] keyValues)
        {
            return Set.Find(keyValues);
        }

        public void Insert(TEntity entity)
        {
            Context.SaveChanges();
        }

        public void Update(TEntity entity)
        {
            Context.SaveChanges();
        }

        /// <summary>
        /// Update an entity of type T and only update the specified properties
        /// </summary>
        /// <typeparam name="T">an entity of type T</typeparam>
        /// <param name="context"></param>
        /// <param name="entityObject">an entity of type T</param>
        /// <param name="properties">a strict list of properties to update</param>
        public void Update<T>(DbContext context, T entityObject, params string[] properties) where T : class
        {
            context.Set<T>().Attach(entityObject); //Attach to the context

            var entry = context.Entry(entityObject); //Get the Entry for this entity

            //Mark each property provided as modified. All properties are initially 
            //assumed to be false - further more properties cannot be marked as false, 
            //this is sadly a limitation of EF 4.3.1
            foreach (string name in properties)
                entry.Property(name).IsModified = true;
        }


        public bool Delete(params object[] keyValues)
        {
            var entity = Find(keyValues);
            if (entity == null)
            {
                return false;
            }
            //code analysis change, unused parameter of ApplyDelete method removed
            ApplyDelete();
            return true;
            Context.SaveChanges();
        }

        /// <summary>
        /// Mark entity as deleted and apply changes to context.
        /// </summary>
        //code analysis change to remove unused parameter
        protected void ApplyDelete()
        {
            Context.SaveChanges();
        }
    }
}

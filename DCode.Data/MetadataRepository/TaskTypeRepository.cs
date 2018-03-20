using System.Collections.Generic;
using DCode.Data.DbContexts;
using DCode.Data.Repository;
using System.Linq;
using System.Linq.Dynamic;
using System.Data.Entity;

namespace DCode.Data.MetadataRepository
{
    public class TaskTypeRepository : Repository<task_type>, ITaskTypeRepository
    {
        private readonly MetadataDbContext _context;

        public TaskTypeRepository(MetadataDbContext context)
            : base(context)
        {
            _context = context;
        }

        public IEnumerable<task_type> GetTaskTypes()
        {
            return Context.Set<task_type>().ToList();
        }
    }
}

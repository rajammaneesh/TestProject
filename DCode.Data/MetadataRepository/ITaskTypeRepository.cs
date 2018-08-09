using DCode.Data.DbContexts;
using System.Collections.Generic;

namespace DCode.Data.MetadataRepository
{
    public interface ITaskTypeRepository
    {
        IEnumerable<task_type> GetTaskTypes();
    }
}

using DCode.Data.DbContexts;
using System.Collections.Generic;

namespace DCode.Data.MetadataRepository
{
    public interface IUserPointsRepository
    {
        IEnumerable<user_points> GetUserPoints();
    }
}

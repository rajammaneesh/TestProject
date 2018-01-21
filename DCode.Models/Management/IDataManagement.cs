using DCode.Models.Common;
using System.Dynamic;

namespace DCode.Models.Management
{
    public interface IDataManagement
    {
        DatabaseTable RunQuery(string query);
    }
}

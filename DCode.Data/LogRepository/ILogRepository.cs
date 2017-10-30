using DCode.Data.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCode.Data.LogRepository
{
    public interface ILogRepository
    {
        int InsertLog(log logModel);
        IEnumerable<log> GetLogs();
    }
}

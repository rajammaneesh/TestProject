using DCode.Models.Common;

namespace DCode.Services.Common
{
    public interface ILoggerService
    {
        int LogToDatabase(Log log);
    }
}

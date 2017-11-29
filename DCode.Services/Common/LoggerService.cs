using DCode.Models.Common;
using DCode.Services.ModelFactory.CommonFactory;
using DCode.Data.DbContexts;
using DCode.Data.LogRepository;

namespace DCode.Services.Common
{
    public class LoggerService : ILoggerService
    {
        private readonly IModelFactory<log> _logModelFactory;

        private readonly ILogRepository _logRepository;

        public LoggerService(IModelFactory<log> logModelFactory, ILogRepository logRepository)
        {
            _logModelFactory = logModelFactory;

            _logRepository = logRepository;
        }

        public int LogToDatabase(Log log)
        {
            var dbLog = _logModelFactory.CreateModel(log);

            var result = _logRepository.InsertLog(dbLog);

            return result;
        }
    }
}

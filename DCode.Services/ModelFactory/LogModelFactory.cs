using DCode.Data.DbContexts;
using DCode.Models.Common;
using DCode.Services.Common;
using DCode.Services.ModelFactory.CommonFactory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCode.Services.ModelFactory
{
    public class LogModelFactory : IModelFactory<log>
    {
        public LogModelFactory()
        {
        }

        public TModel CreateModel<TModel>(log input) where TModel : class
        {
            var logModel = new Log();
            logModel.Description = input.description;
            logModel.Details = input.details;
            logModel.Id = input.Id;
            return logModel as TModel;
        }

        public log CreateModel<TModel>(log input, TModel model) where TModel : class
        {
            throw new NotImplementedException();
        }

        public log CreateModel<TModel>(TModel input) where TModel : class
        {
            if (input.GetType() == typeof(Log))
            {
                var logDb = new log();
                var inputModel = input as Log;
                logDb.description = inputModel.Description;
                //logDb.details = inputModel.Details;
                logDb.details = DateTime.Now.ToString();
                //logDb.user = _commonService.GetCurrentUserContext().Name;
                return logDb;
            }
            return null;
        }

        public IEnumerable<TModel> CreateModelList<TModel>(IEnumerable<log> inputList) where TModel : class
        {
            var modelList = new List<Log>();
            foreach (var dbTask in inputList)
            {
                var log = new Log();
                log = CreateModel<Log>(dbTask);
                modelList.Add(log);
            }
            return modelList as IEnumerable<TModel>;
        }
    }
}

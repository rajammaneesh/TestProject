using DCode.Common;
using DCode.Data.DbContexts;
using DCode.Services.ModelFactory.CommonFactory;
using System;
using System.Collections.Generic;
using static DCode.Models.Enums.Enums;

namespace DCode.Services.ModelFactory
{
    public class TaskSubOfferingMapModelFactory : IModelFactory<string>
    {
        public TModel CreateModel<TModel>(string subOfferingId) where TModel : class
        {
            if (!string.IsNullOrEmpty(subOfferingId) && Convert.ToInt32(subOfferingId) > 0)
            {
                var item = new task_suboffering_map();
                item.SUB_OFFERING_ID = Convert.ToInt32(subOfferingId);
                return item as TModel;
            }
            return null;
        }

        public IEnumerable<TModel> CreateModelList<TModel>(IEnumerable<string> inputList) where TModel : class
        {
            throw new NotImplementedException();
        }

        string IModelFactory<string>.CreateModel<TModel>(string input, TModel model)
        {
            throw new NotImplementedException();
        }

        string IModelFactory<string>.CreateModel<TModel>(TModel input)
        {
            throw new NotImplementedException();
        }

       
    }
}

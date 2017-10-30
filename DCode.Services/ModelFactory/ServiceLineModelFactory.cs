using DCode.Data.DbContexts;
using DCode.Models.ResponseModels.Common;
using DCode.Services.ModelFactory.CommonFactory;
using System;
using System.Collections.Generic;

namespace DCode.Services.ModelFactory
{
    public class ServiceLineModelFactory : IModelFactory<service_line>
    {
        public service_line CreateModel<TModel>(TModel input) where TModel : class
        {
            throw new NotImplementedException();
        }

        public TModel CreateModel<TModel>(service_line input) where TModel : class
        {
            if (typeof(TModel) == typeof(ServiceLine))
            {
                return TranslateToOutput(input) as TModel;
            }
            return null;
        }

        public service_line CreateModel<TModel>(service_line input, TModel model) where TModel : class
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TModel> CreateModelList<TModel>(IEnumerable<service_line> inputList) where TModel : class
        {
            if (typeof(TModel) == typeof(ServiceLine))
            {
                if (inputList == null)
                {
                    return null;
                }

                var listOfServiceLines = new List<ServiceLine>();

                foreach (var item in inputList)
                {
                    listOfServiceLines.Add(TranslateToOutput(item));
                }

                return listOfServiceLines as IEnumerable<TModel>;
            }
            return null;
        }

        private ServiceLine TranslateToOutput(service_line input)
        {
            return new ServiceLine
            {
                Id = input.ID,
                Description = input.Description,
                Name = input.Name
            };
        }
    }
}

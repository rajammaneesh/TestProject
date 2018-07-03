using DCode.Data.DbContexts;
using DCode.Models.ResponseModels.Common;
using DCode.Services.ModelFactory.CommonFactory;
using System;
using System.Collections.Generic;

namespace DCode.Services.ModelFactory
{
    public class PortfolioModelFactory : IModelFactory<portfolio>
    {
        public portfolio CreateModel<TModel>(TModel input) where TModel : class
        {
            throw new NotImplementedException();
        }

        public TModel CreateModel<TModel>(portfolio input) where TModel : class
        {
            if (typeof(TModel) == typeof(Portfolio))
            {
                return TranslateToOutput(input) as TModel;
            }
            return null;
        }

        public portfolio CreateModel<TModel>(portfolio input, TModel model) where TModel : class
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TModel> CreateModelList<TModel>(IEnumerable<portfolio> inputList) where TModel : class
        {
            if (typeof(TModel) == typeof(Portfolio))
            {
                if (inputList == null)
                {
                    return null;
                }

                var listOfPortfolios = new List<Portfolio>();

                foreach (var item in inputList)
                {
                    listOfPortfolios.Add(TranslateToOutput(item));
                }

                return listOfPortfolios as IEnumerable<TModel>;
            }
            return null;
        }

        private Portfolio TranslateToOutput(portfolio input)
        {
            return new Portfolio
            {
                Id = input.Id,
                Description = input.Description,
                Code = input.Code
            };
        }
    }
}

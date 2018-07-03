using DCode.Data.DbContexts;
using DCode.Models.ResponseModels.Common;
using DCode.Services.ModelFactory.CommonFactory;
using System;
using System.Collections.Generic;

namespace DCode.Services.ModelFactory
{
    public class OfferingModelFactory : IModelFactory<offering>
    {
        public offering CreateModel<TModel>(TModel input) where TModel : class
        {
            throw new NotImplementedException();
        }

        public TModel CreateModel<TModel>(offering input) where TModel : class
        {
            if (typeof(TModel) == typeof(Offering))
            {
                return TranslateToOutput(input) as TModel;
            }
            return null;
        }

        public offering CreateModel<TModel>(offering input, TModel model) where TModel : class
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TModel> CreateModelList<TModel>(IEnumerable<offering> inputList) where TModel : class
        {
            if (typeof(TModel) == typeof(Offering))
            {
                if (inputList == null)
                {
                    return null;
                }

                var listOfOfferings = new List<Offering>();

                foreach (var item in inputList)
                {
                    listOfOfferings.Add(TranslateToOutput(item));
                }

                return listOfOfferings as IEnumerable<TModel>;
            }
            return null;
        }

        private Offering TranslateToOutput(offering input)
        {
            return new Offering
            {
                Id = input.Id,
                Description = input.Description,
                PortfolioId = input.Portfolio_Id,
                Code = input.Code,
                RMEmailGroup = input.RM_Email_Group,
                PracticeEmailGroup = input.Practice_Email_Group
            };
        }
    }
}

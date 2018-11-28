using DCode.Data.DbContexts;
using DCode.Models.ResponseModels.Common;
using System.Collections.Generic;

namespace DCode.Services.ModelFactory
{
    public class SubOfferingModelFactory
    {
        public TModel CreateModel<TModel>(suboffering input) where TModel : class
        {
            if (typeof(TModel) == typeof(SubOffering))
            {
                return TranslateToOutput(input) as TModel;
            }
            return null;
        }

        public IEnumerable<TModel> CreateModelList<TModel>(IEnumerable<suboffering> inputList) where TModel : class
        {
            if (typeof(TModel) == typeof(SubOffering))
            {
                if (inputList == null)
                {
                    return null;
                }

                var listOfOfferings = new List<SubOffering>();

                foreach (var item in inputList)
                {
                    listOfOfferings.Add(TranslateToOutput(item));
                }

                return listOfOfferings as IEnumerable<TModel>;
            }
            return null;
        }

        private SubOffering TranslateToOutput(suboffering input)
        {
            return new SubOffering
            {
                Id = input.ID,
                Description = input.Name,
                Code = input.Code,
                OfferingId = input.OfferingsId,
                Practice_Email_Group = input.Practice_Email_Group
            };
        }
    }
}

using DCode.Data.DbContexts;
using DCode.Models.Common;
using DCode.Services.ModelFactory.CommonFactory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCode.Services.ModelFactory
{
    public class SuggestionModelFactory : IModelFactory<suggestion>
    {
        public TModel CreateModel<TModel>(suggestion input) where TModel : class
        {
            if (typeof(TModel) == typeof(Suggestion))
            {
                return TranslateToOutput(input) as TModel;
            }
            return null;
        }

        private Suggestion TranslateToOutput(suggestion input)
        {
            var output = new Suggestion();
            output.Description = input.description;
            output.Details = input.description;
            output.Id = input.Id;
            output.user = input.user;
            return output;
        }

        public suggestion CreateModel<TModel>(suggestion input, TModel model) where TModel : class
        {
            throw new NotImplementedException();
        }

        public suggestion CreateModel<TModel>(TModel input) where TModel : class
        {
            if(typeof(TModel) == typeof(Suggestion))
            {
                return TranslateToDb(input as Suggestion);
            }
            return null;
        }

        private suggestion TranslateToDb(Suggestion input)
        {
            var suggestion = new suggestion();
            suggestion.description = input.Description;
            suggestion.details = input.Details;
            suggestion.Id = input.Id;
            suggestion.user = input.user;
            return suggestion;
        }

        public IEnumerable<TModel> CreateModelList<TModel>(IEnumerable<suggestion> inputList) where TModel : class
        {
            var outputList = new List<Suggestion>();
            foreach (var input in inputList)
            {
                var dbInput = CreateModel<Suggestion>(input);
                outputList.Add(dbInput);
            }
            return outputList as IEnumerable<TModel>;
        }
    }
}

using DCode.Data.DbContexts;
using DCode.Models.ResponseModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCode.Services.ModelFactory
{
    public class UserPointsModelFactory
    {
        public TModel CreateModel<TModel>(user_points input) where TModel : class
        {
            if (typeof(TModel) == typeof(UserPoints))
            {
                return TranslateToOutput(input) as TModel;
            }
            return null;
        }

        public IEnumerable<TModel> CreateModelList<TModel>(IEnumerable<user_points> inputList) where TModel : class
        {
            if (typeof(TModel) == typeof(UserPoints))
            {
                if (inputList == null)
                {
                    return null;
                }

                var listOfOfferings = new List<UserPoints>();

                foreach (var item in inputList)
                {
                    listOfOfferings.Add(TranslateToOutput(item));
                }

                return listOfOfferings as IEnumerable<TModel>;
            }
            return null;
        }

        private UserPoints TranslateToOutput(user_points input)
        {
            return new UserPoints
            {
                Id = input.Id,
                user_id = input.user_id,
                points = input.points
            };
        }
    }
}

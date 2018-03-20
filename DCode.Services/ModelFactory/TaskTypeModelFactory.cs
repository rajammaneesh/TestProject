using DCode.Data.DbContexts;
using DCode.Models.ResponseModels.Common;
using DCode.Services.ModelFactory.CommonFactory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCode.Services.ModelFactory
{
    public class TaskTypeModelFactory : IModelFactory<task_type>
    {
        public task_type CreateModel<TModel>(TModel input) where TModel : class
        {
            throw new NotImplementedException();
        }

        public TModel CreateModel<TModel>(task_type input) where TModel : class
        {
            if (typeof(TModel) == typeof(TaskType))
            {
                return TranslateToOutput(input) as TModel;
            }
            return null;
        }

        public task_type CreateModel<TModel>(task_type input, TModel model) where TModel : class
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TModel> CreateModelList<TModel>(IEnumerable<task_type> inputList) where TModel : class
        {
            if (typeof(TModel) == typeof(TaskType))
            {
                if (inputList == null)
                {
                    return null;
                }

                var listOfTaskTypes = new List<TaskType>();

                foreach (var item in inputList)
                {
                    listOfTaskTypes.Add(TranslateToOutput(item));
                }

                return listOfTaskTypes as IEnumerable<TModel>;
            }
            return null;
        }

        private TaskType TranslateToOutput(task_type input)
        {
            return new TaskType
            {
                Id = input.ID,
                Description = input.DESCRIPTION,
                Code = input.CODE
            };
        }
    }
}

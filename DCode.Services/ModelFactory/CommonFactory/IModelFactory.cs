using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCode.Services.ModelFactory.CommonFactory
{
    public interface IModelFactory<TEntity>
    {
        TModel CreateModel<TModel>(TEntity input) where TModel : class;
        TEntity CreateModel<TModel>(TEntity input, TModel model) where TModel : class;
        TEntity CreateModel<TModel>(TModel input) where TModel : class;

        IEnumerable<TModel> CreateModelList<TModel>(IEnumerable<TEntity> inputList) where TModel : class;
    }
}

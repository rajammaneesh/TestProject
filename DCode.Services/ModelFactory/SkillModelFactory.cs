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
    public class SkillModelFactory :IModelFactory<skill>
    {
        public TModel CreateModel<TModel>(skill input) where TModel : class
        {
            var skill = new Skill();
            skill.Id = input.ID;
            skill.Value = input.VALUE;
            return skill as TModel;
        }

        public skill CreateModel<TModel>(skill input, TModel model) where TModel : class
        {
            throw new NotImplementedException();
        }

        public skill CreateModel<TModel>(TModel input) where TModel : class
        {
            if(typeof(TModel) == typeof(Skill))
            {
                return TranslateSkill(input as Skill);
            }
            return null;
        }

        private skill TranslateSkill(Skill input)
        {
            var dbSkill = new skill();
            dbSkill.ID = input.Id;
            dbSkill.VALUE = input.Value;
            return dbSkill;
        }

        public IEnumerable<TModel> CreateModelList<TModel>(IEnumerable<skill> inputList) where TModel : class
        {
            var skillSets = new List<Skill>();
            foreach(var skill in inputList)
            {
                skillSets.Add(CreateModel<Skill>(skill));
            }
            return skillSets as IEnumerable<TModel>;
        }
    }
}

using DCode.Data.DbContexts;
using DCode.Models.ResponseModels.Common;
using DCode.Services.ModelFactory.CommonFactory;
using System;
using System.Collections.Generic;
using static DCode.Models.Enums.Enums;

namespace DCode.Services.ModelFactory
{
    public class ApplicantSkillModelFactory : IModelFactory<applicantskill>
    {
        public TModel CreateModel<TModel>(applicantskill input) where TModel : class
        {
            throw new NotImplementedException();
        }

        public applicantskill CreateModel<TModel>(applicantskill input, TModel model) where TModel : class
        {
            throw new NotImplementedException();
        }

        public applicantskill CreateModel<TModel>(TModel input) where TModel : class
        {
            return null;
        }

        public IEnumerable<applicantskill> CreateModel<TModel>(List<TModel> input,int userId) where TModel : class
        {
            if (input != null)
            {
                var applicantSkillList = new List<applicantskill>();
                foreach (var skill in input as IEnumerable<Skill>)
                {
                    var applicantSkill = new applicantskill();
                    applicantSkill.APPLICANT_ID = userId;
                    applicantSkill.SKILL_ID = skill.Id;
                    applicantSkill.STATUS = ApplicantStatus.Active.ToString();
                    applicantSkill.STATUS_DATE = DateTime.Now;
                    applicantSkillList.Add(applicantSkill);
                }
                return applicantSkillList;
            }
            return null;
        }

        public IEnumerable<TModel> CreateModelList<TModel>(IEnumerable<applicantskill> inputList) where TModel : class
        {
            throw new NotImplementedException();
        }
    }
}

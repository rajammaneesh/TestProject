using DCode.Common;
using DCode.Data.DbContexts;
using DCode.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCode.Services.Base
{
    public class BaseService
    {
        protected string ConvertSkillsToString(IEnumerable<applicantskill> applicantSkills)
        {
            var skillsCompliation = string.Empty;
            foreach(var applicantSkill in applicantSkills)
            {
                if (applicantSkill.skill != null)
                {
                    if (skillsCompliation.Equals(string.Empty))
                    {
                        skillsCompliation = applicantSkill.skill.VALUE;
                    }
                    else
                    {
                        skillsCompliation = skillsCompliation + Constants.Comma + Constants.Space + applicantSkill.skill.VALUE;
                    }
                }
            }
            return skillsCompliation;
        }
        protected void MapAuditFields<TEntity>(Enums.ActionType action, TEntity model) where TEntity : class
        {
            var userContext = SessionHelper.Retrieve(Constants.UserContext) as UserContext;
            if (userContext != null && action == Enums.ActionType.Insert)
            {
                if (typeof(TEntity) == typeof(task))
                {
                    var entity = model as task;
                    entity.CREATED_BY = userContext.EmailId;
                    entity.CREATED_ON = DateTime.Now;
                }
                if (typeof(TEntity) == typeof(user))
                {
                    var entity = model as user;
                    entity.CREATED_BY = userContext.EmailId;
                    entity.CREATED_ON = DateTime.Now;
                }
                if (typeof(TEntity) == typeof(approvedapplicant))
                {
                    var entity = model as approvedapplicant;
                    entity.CREATED_BY = userContext.Name;
                    entity.CREATED_ON = DateTime.Now;
                }
                if (typeof(TEntity) == typeof(taskapplicant))
                {
                    var entity = model as taskapplicant;
                    entity.CREATED_BY = userContext.EmailId;
                    entity.CREATED_ON = DateTime.Now;
                }
                if (typeof(TEntity) == typeof(taskskill))
                {
                    var entity = model as taskskill;
                    entity.CREATED_BY = userContext.EmailId;
                    entity.CREATED_ON = DateTime.Now;
                }
            }
            else if (userContext != null && action == Enums.ActionType.Update)
            {
                if (typeof(TEntity) == typeof(task))
                {
                    var entity = model as task;
                    entity.UPDATED_BY = userContext.EmailId;
                    entity.UPDATED_ON = DateTime.Now;
                }
                if (typeof(TEntity) == typeof(user))
                {
                    var entity = model as user;
                    entity.UPDATED_BY = userContext.EmailId;
                    entity.UPDATED_ON = DateTime.Now;
                }
                if (typeof(TEntity) == typeof(approvedapplicant))
                {
                    var entity = model as approvedapplicant;
                    entity.UPDATED_BY = userContext.EmailId;
                    entity.UPDATED_ON = DateTime.Now;
                }
                if (typeof(TEntity) == typeof(taskapplicant))
                {
                    var entity = model as taskapplicant;
                    entity.UPDATED_BY = userContext.EmailId;
                    entity.UPDATED_ON = DateTime.Now;
                }
                if (typeof(TEntity) == typeof(taskskill))
                {
                    var entity = model as taskskill;
                    entity.UPDATED_BY = userContext.EmailId;
                    entity.UPDATED_ON = DateTime.Now;
                }
            }
        }
    }
}

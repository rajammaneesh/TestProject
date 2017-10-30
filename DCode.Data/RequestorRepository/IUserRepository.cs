using DCode.Data.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCode.Data.RequestorRepository
{
    public interface IUserRepository
    {
        IEnumerable<applicantskill> GetSkillsByUserId(int userId);
        int InsertUser(user user);
        user GetUserByEmailId(string emailId);
        int InsertTaskApplicant(taskapplicant taskApplicant);
        int InsertApplicantAndTask(taskapplicant taskApplicant, user applicant);
        int UpdateProfile(user user, IEnumerable<applicantskill> applicantSkills);
        skill GetSkillByName(string skillName);
        int AddNewSkill(skill skill);
        int AddSuggestion(suggestion suggestion);
        IEnumerable<suggestion> GetSuggestions();
    }
}

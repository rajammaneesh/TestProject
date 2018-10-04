using DCode.Data.DbContexts;
using System.Collections.Generic;

namespace DCode.Data.UserRepository
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
        int UpdateManager(int userId, string managerName, string managerEmailId);
        IEnumerable<string> GetSubscribedUserForTask(string task);

        IEnumerable<string> GetAllActiveUsers();

        IEnumerable<user> GetAllActiveUsersDetails();
        IEnumerable<user_locations> GetAllLocations();
        IEnumerable<user> GetAllUsers();
        IEnumerable<user_locations> GetAllUser_Locations();
        int UpdateOfferingIdForUser(int userId, int? offeringId);

        int UpdateLocationForUser(int userId, int? locationId);        
    }
}

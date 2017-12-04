using DCode.Models;
using DCode.Models.Common;
using DCode.Models.RequestModels;
using DCode.Models.ResponseModels.Common;
using DCode.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DCode.Services.Common
{
    public interface ICommonService
    {
        UserContext GetCurrentUserContext(string userName = null);

        int LogToDatabase(Log logmodel);
        IEnumerable<Log> GetDBLogs();
        UserContext SwitchRole();
        int InsertApplicant(ApplicantRequest model);
        int UpdateProfile(ProfileRequest profileRequest);
        IEnumerable<Skill> SearchSkill(string searchParam);
        string InsertNewSkill(string skillValue);
        UserContext SwitchRole(string roleFromUI);
        int InsertNewSuggestion(string suggestion);
        IEnumerable<Suggestion> GetSuggestions();

        IEnumerable<ServiceLine> GetServiceLines();

        string GetNameFromEmailId(string emailId);

        int UpdateManagersEmail(string usersEmailAddress, string managersEmailAddress, string managersName);

        UserContext MapDetailsFromDeloitteNetworkWithoutUserContextObject(string userName);

        bool GetTechXAccess();

        string GetRMGroupEmailAddress(string department);
    }
}

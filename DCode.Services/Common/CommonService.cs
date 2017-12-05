using DCode.Data.TaskRepository;
using DCode.Models.User;
using DCode.Services.ModelFactory;
using System;
using System.Collections.Generic;
using System.Text;
using System.DirectoryServices;
using DCode.Common;
using System.Configuration;
using DCode.Data.DbContexts;
using Elmah;
using DCode.Models.Common;
using DCode.Data.LogRepository;
using DCode.Models.RequestModels;
using DCode.Data.RequestorRepository;
using DCode.Models.ResponseModels.Common;
using DCode.Services.Base;
using DCode.Data.MetadataRepository;
using System.Text.RegularExpressions;
using static DCode.Models.Enums.Enums;
using System.Linq;
using DCode.Data.UserRepository;

namespace DCode.Services.Common
{
    public class CommonService : BaseService, ICommonService
    {
        private ITaskRepository _taskRepository;
        private ILogRepository _logRepository;
        //private TaskModelFactory _taskModelFactory;
        private IRequestorRepository _requestorRepository;
        private IUserRepository _userRepository;
        private UserContext _userContext;
        private LogModelFactory _logModelFactory;
        private UserModelFactory _userModelFactory;
        private ApplicantSkillModelFactory _applicantSkillModelFactory;
        private SkillModelFactory _skillModelFactory;
        private SuggestionModelFactory _suggestionModelFactory;
        private IServiceLineRepository _serviceLineRepository;
        private ServiceLineModelFactory _serviceLineModelFactory;

        public CommonService(ITaskRepository taskRepository, UserContext userContext, ILogRepository logRepository, LogModelFactory logModelFactory, IRequestorRepository requestorRepository, IUserRepository userRepository, UserModelFactory userModelFactory, ApplicantSkillModelFactory applicantSkillModelFactory, SkillModelFactory skillModelFactory, SuggestionModelFactory suggestionModelFactory, IServiceLineRepository serviceLineRepository, ServiceLineModelFactory serviceLineModelFactory)
        {
            _taskRepository = taskRepository;
            _logModelFactory = logModelFactory;
            _userContext = userContext;
            _logRepository = logRepository;
            _requestorRepository = requestorRepository;
            _userRepository = userRepository;
            _userModelFactory = userModelFactory;
            _applicantSkillModelFactory = applicantSkillModelFactory;
            _skillModelFactory = skillModelFactory;
            _suggestionModelFactory = suggestionModelFactory;
            _serviceLineRepository = serviceLineRepository;
            _serviceLineModelFactory = serviceLineModelFactory;
        }

        public UserContext GetCurrentUserContext(string userName = null)
        {
            if (SessionHelper.Retrieve(Constants.MockUser) != null)
            {
                if (SessionHelper.Retrieve(Constants.UserContext) != null)
                {
                    _userContext = SessionHelper.Retrieve(Constants.UserContext) as UserContext;
                }
                else
                {
                    _userContext = SessionHelper.Retrieve(Constants.MockUser) as UserContext;
                    SetAndInsertContext();
                }
            }
            else
            {
                if (SessionHelper.Retrieve(Constants.UserContext) != null)
                {
                    _userContext = SessionHelper.Retrieve(Constants.UserContext) as UserContext;
                }
                else
                {
                    try
                    {
                        _userContext = MapDetailsFromDeloitteNetwork(userName);
                        SetAndInsertContext();
                    }
                    catch (Exception ex)
                    {
                        ErrorSignal.FromCurrentContext().Raise(ex);
                        if (SessionHelper.Retrieve(Constants.MockUser) != null)
                        {
                            _userContext = (UserContext)SessionHelper.Retrieve(Constants.MockUser);
                        }
                        else
                        {
                            _userContext = SessionHelper.Retrieve(Constants.MockUser) as UserContext;
                            SetAndInsertContext();
                        }
                    }
                }
            }

            if (_userContext.MenuItems == null)
            {
                _userContext.MenuItems = FetchMenuItems(_userContext.Role);
            }
            return _userContext;
        }

        private UserContext MapDetailsFromDeloitteNetwork(string userName)
        {
            SearchResultCollection searchResults = null;
            string path = string.Format(ConfigurationManager.AppSettings[Constants.LdapConnection].ToString(), userName);
            using (var directoryEntry = new DirectoryEntry(path))
            using (var directorySearcher = new DirectorySearcher(directoryEntry))
            {
                directorySearcher.Filter = string.Format(Constants.SearchFilter, userName);
                searchResults = directorySearcher.FindAll();
                var propertyNames = searchResults[0].Properties.PropertyNames as List<ResultPropertyCollection>;

                var propertyDescription = new StringBuilder();

                foreach (SearchResult result in searchResults)
                {
                    foreach (string propertyName in result.Properties.PropertyNames)
                    {
                        if (propertyName.ToLowerInvariant().Equals(Constants.Userprincipalname))
                        {
                            _userContext.EmailId = result.Properties[propertyName][0].ToString();
                        }
                        else if (propertyName.ToLowerInvariant().Equals(Constants.Title))
                        {
                            _userContext.Designation = result.Properties[propertyName][0].ToString();
                        }
                        else if (propertyName.ToLowerInvariant().Equals(Constants.Givenname))
                        {
                            _userContext.FirstName = result.Properties[propertyName][0].ToString();
                        }
                        else if (propertyName.ToLowerInvariant().Equals(Constants.SN))
                        {
                            _userContext.LastName = result.Properties[propertyName][0].ToString();
                        }
                        else if (propertyName.ToLowerInvariant().Equals(Constants.Name))
                        {
                            _userContext.EmailId = result.Properties[propertyName][0].ToString() + Constants.DeloitteEmailExtn;
                        }
                        else if (propertyName.ToLowerInvariant().Equals(Constants.EmployeeId))
                        {
                            _userContext.EmployeeId = result.Properties[propertyName][0].ToString();
                        }
                        else if (propertyName.ToLowerInvariant().Equals(Constants.TelephoneNumber))
                        {
                            _userContext.TelephoneNumber = result.Properties[propertyName][0].ToString();
                        }
                        else if (propertyName.ToLowerInvariant().Equals(Constants.Department))
                        {
                            _userContext.Department = result.Properties[propertyName][0].ToString();
                        }
                        else if ((propertyName.ToLowerInvariant().Equals(Constants.MsArchiveName)))
                        {
                            _userContext.MsArchiveName = result.Properties[propertyName][0].ToString();
                        }
                    }
                }
                return _userContext;
            }
        }

        public UserContext MapDetailsFromDeloitteNetworkWithoutUserContextObject(string userName)
        {
            var userContext = new UserContext();
            SearchResultCollection searchResults = null;
            string path = string.Format(ConfigurationManager.AppSettings[Constants.LdapConnection].ToString(), userName);

            using (var directoryEntry = new DirectoryEntry(path))
            using (var directorySearcher = new DirectorySearcher(directoryEntry))
            {
                directorySearcher.Filter = string.Format(Constants.SearchFilter, userName);
                searchResults = directorySearcher.FindAll();
                var propertyNames = searchResults[0].Properties.PropertyNames as List<ResultPropertyCollection>;

                var propertyDescription = new StringBuilder();
                foreach (SearchResult result in searchResults)
                {
                    foreach (string propertyName in result.Properties.PropertyNames)
                    {
                        if (propertyName.ToLowerInvariant().Equals(Constants.Userprincipalname))
                        {
                            userContext.EmailId = result.Properties[propertyName][0].ToString();
                        }
                        else if (propertyName.ToLowerInvariant().Equals(Constants.Title))
                        {
                            userContext.Designation = result.Properties[propertyName][0].ToString();
                        }
                        else if (propertyName.ToLowerInvariant().Equals(Constants.Givenname))
                        {
                            userContext.FirstName = result.Properties[propertyName][0].ToString();
                        }
                        else if (propertyName.ToLowerInvariant().Equals(Constants.SN))
                        {
                            userContext.LastName = result.Properties[propertyName][0].ToString();
                        }
                        else if (propertyName.ToLowerInvariant().Equals(Constants.Name))
                        {
                            userContext.EmailId = result.Properties[propertyName][0].ToString() + Constants.DeloitteEmailExtn;
                        }
                        else if (propertyName.ToLowerInvariant().Equals(Constants.EmployeeId))
                        {
                            userContext.EmployeeId = result.Properties[propertyName][0].ToString();
                        }
                        else if (propertyName.ToLowerInvariant().Equals(Constants.TelephoneNumber))
                        {
                            userContext.TelephoneNumber = result.Properties[propertyName][0].ToString();
                        }
                        else if (propertyName.ToLowerInvariant().Equals(Constants.Department))
                        {
                            userContext.Department = result.Properties[propertyName][0].ToString();
                        }
                    }
                }
                return userContext;
            }
        }

        private void SetAndInsertContext()
        {
            if (_userContext.Designation.Contains("senior manager") || _userContext.Designation.Contains("specialist leader") || _userContext.Designation.Contains("director") || _userContext.Designation.Contains("partner"))
            {
                //_userContext.Role = Enums.Role.Admin;
                _userContext.Role = Role.Requestor;
                _userContext.IsCoreRoleRequestor = true;
            }
            else if (_userContext.Designation.ToLowerInvariant().Contains("manager") || _userContext.Designation.ToLowerInvariant().Contains("master"))
            {
                _userContext.Role = Role.Requestor;
                _userContext.IsCoreRoleRequestor = true;
            }
            else
            {
                _userContext.Role = Role.Contributor;
                _userContext.IsCoreRoleRequestor = false;
            }
            var dbUser = _requestorRepository.GetUserByEmailId(_userContext.EmailId);
            if (dbUser != null && dbUser.ID != null)
            {
                _userContext.UserId = dbUser.ID;
                _userContext.ManagerEmailId = dbUser.MANAGER_EMAIL_ID;
                _userContext.ProjectCode = dbUser.PROJECT_CODE;
                _userContext.ManagerName = dbUser.PROJECT_MANAGER_NAME;
                _userContext.ProjectName = dbUser.PROJECT_NAME;
                _userContext.SkillSet = new List<Skill>();
                foreach (var dbSkill in dbUser.applicantskills)
                {
                    var skill = new Skill();
                    skill.Id = dbSkill.skill.ID;
                    skill.Value = dbSkill.skill.VALUE;
                    _userContext.SkillSet.Add(skill);
                }

                if (dbUser.notification_subscription != null && dbUser.notification_subscription.Any())
                {
                    _userContext.IsSubscribedToNotifications 
                        = dbUser.notification_subscription.First().subscription_status;
                }
            }
            else
            {
                var userDbObj = _userModelFactory.CreateModel(_userContext);
                var result = _userRepository.InsertUser(userDbObj);
                var dbUserres = _requestorRepository.GetUserByEmailId(_userContext.EmailId);
                _userContext.UserId = dbUserres.ID;
            }
        }

        private List<MenuItem> FetchMenuItems(Role role)
        {
            var menuItemsList = new List<MenuItem>();

            switch (_userContext.Role)
            {
                case Role.Admin:

                    break;
                case Role.Requestor:
                    menuItemsList.Add(new MenuItem() { MenuItemName = "CREATE NEW TASK", TabName = Constants.TabNewTask, NavigationUrl = "/Requestor/NewTasks", CssClass = "" });
                    menuItemsList.Add(new MenuItem() { MenuItemName = "MY TASKS", TabName = Constants.TabMyTasks, NavigationUrl = "/Requestor/Dashboard", ImageUrlActive = "/Content/Images/dashboard@2x.png", ImageUrlInactive = "/Content/Images/dashboard-disabled@2x.png", CssClass = "mytask-icon" });
                    menuItemsList.Add(new MenuItem() { MenuItemName = "APPROVALS", TabName = Constants.TabPermissions, NavigationUrl = "/Requestor/Permissions", ImageUrlActive = "/Content/Images/permission-icon.png", ImageUrlInactive = "/Content/Images/person-disable.png", CssClass = "permission-icon" });
                    menuItemsList.Add(new MenuItem() { MenuItemName = "HISTORY", TabName = Constants.TabHistory, NavigationUrl = "/Requestor/History", ImageUrlActive = "/Content/Images/history-active.png", ImageUrlInactive = "/Content/Images/history-icon.png", CssClass = "history-icon" });
                    menuItemsList.Add(new MenuItem() { MenuItemName = "CONTACT US", TabName = Constants.ContactUS, NavigationUrl = "/ContactUs/ContactUs", CssClass = "" });
                    break;
                case Role.Contributor:
                    menuItemsList.Add(new MenuItem() { MenuItemName = "MY TASKS", TabName = Constants.TabMyTasks, NavigationUrl = "/Contributor/Dashboard", ImageUrlActive = "/Content/Images/dashboard@2x.png", ImageUrlInactive = "/Content/Images/dashboard-disabled@2x.png", CssClass = "mytask-icon" });
                    menuItemsList.Add(new MenuItem() { MenuItemName = "HISTORY", TabName = Constants.TabHistory, NavigationUrl = "/Contributor/History", ImageUrlActive = "/Content/Images/history-active.png", ImageUrlInactive = "/Content/Images/history-icon.png", CssClass = "history-icon" });
                    menuItemsList.Add(new MenuItem() { MenuItemName = "CONTACT US", TabName = Constants.ContactUS, NavigationUrl = "/ContactUs/ContactUs",  CssClass = "" });

                    break;
                default:
                    break;
            }
            return menuItemsList;
        }

        public UserContext SwitchRole()
        {
            var userContext = GetCurrentUserContext();
            userContext.Role = (userContext.Role == Role.Contributor) ? Role.Requestor : Role.Contributor;
            userContext.MenuItems = null;
            userContext.MenuItems = FetchMenuItems(userContext.Role);
            SessionHelper.Save(Constants.UserContext, userContext);
            return userContext;
        }

        public UserContext SwitchRole(string roleFromUI)
        {
            var userContext = GetCurrentUserContext();
            if (roleFromUI.ToLowerInvariant().Equals(Role.Requestor.ToString().ToLowerInvariant()))
            {
                userContext.Role = Role.Requestor;
            }
            else
            {
                userContext.Role = Role.Contributor;
            }
            userContext.MenuItems = null;
            userContext.MenuItems = FetchMenuItems(userContext.Role);
            SessionHelper.Save(Constants.UserContext, userContext);
            return userContext;
        }

        public int LogToDatabase(Log logmodel)
        {
            var dbLog = _logModelFactory.CreateModel(logmodel);
            var result = _logRepository.InsertLog(dbLog);
            return result;
        }

        public IEnumerable<Log> GetDBLogs()
        {
            var logs = _logRepository.GetLogs();
            var result = _logModelFactory.CreateModelList<Log>(logs);
            return result;
        }

        public int InsertApplicant(ApplicantRequest model)
        {
            int result = 0;
            var applicant = _userRepository.GetUserByEmailId(model.EmailId);
            if (applicant != null && applicant.ID != null)
            {
                var taskApplicant = new taskapplicant();
                taskApplicant.APPLICANT_ID = applicant.ID;
                taskApplicant.TASK_ID = model.TaskId;
                taskApplicant.STATUS = TaskApplicant.Active.ToString();
                result = _userRepository.InsertTaskApplicant(taskApplicant);
            }
            else
            {
                var applicantt = new user();
                applicantt.EMAIL_ID = model.EmailId;
                applicantt.FIRST_NAME = model.FirstName;
                applicantt.LAST_NAME = model.LastName;
                applicantt.MANAGER_EMAIL_ID = model.ManagerEmailId;

                var taskApplicant = new taskapplicant();
                taskApplicant.TASK_ID = model.TaskId;
                taskApplicant.STATUS = TaskApplicant.Active.ToString();
                result = _userRepository.InsertApplicantAndTask(taskApplicant, applicantt);
            }
            return result;
        }

        public int UpdateProfile(ProfileRequest profileRequest)
        {
            var user = _userModelFactory.CreateModel<ProfileRequest>(profileRequest);
            MapAuditFields<user>(ActionType.Insert, user);
            var applicantSkills = _applicantSkillModelFactory.CreateModel<Skill>(profileRequest.SkillSet, profileRequest.UserId);
            if (applicantSkills != null)
            {
                foreach (var skill in applicantSkills)
                {
                    MapAuditFields<applicantskill>(ActionType.Insert, skill);
                }
            }
            var result = _userRepository.UpdateProfile(user, applicantSkills);
            _userContext = (UserContext)SessionHelper.Retrieve(Constants.UserContext);
            SetAndInsertContext();
            return result;
        }

        public IEnumerable<Skill> SearchSkill(string searchParam)
        {
            var skillsDb = _requestorRepository.SearchSkill(searchParam);
            var skills = _skillModelFactory.CreateModelList<Skill>(skillsDb);
            return skills;
        }

        public string InsertNewSkill(string skillValue)
        {
            var doesSkillExist = _userRepository.GetSkillByName(skillValue);
            if (doesSkillExist != null && doesSkillExist.ID != null)
            {
                return ErrorMessages.SkillExists.ToString();
            }
            else
            {
                var user = GetCurrentUserContext();
                var dbSkill = new skill();
                dbSkill.CREATED_BY = user.EmailId;
                dbSkill.CREATED_ON = DateTime.Now;
                dbSkill.STATUS = SkillStatus.Active.ToString();
                dbSkill.STATUS_DATE = DateTime.Now;
                dbSkill.VALUE = skillValue;

                var result = _userRepository.AddNewSkill(dbSkill);
                if (result > 0)
                {
                    return SuccessMessages.SkillAdded.ToString();
                }
                return ErrorMessages.SkillExists.ToString();
            }
        }

        public int InsertNewSuggestion(string suggestion)
        {
            var user = GetCurrentUserContext();
            var dbSuggestion = new suggestion();
            dbSuggestion.description = suggestion;
            dbSuggestion.details = user.Name;
            dbSuggestion.user = user.EmailId;
            var result = _userRepository.AddSuggestion(dbSuggestion);
            return result;
        }

        public IEnumerable<Suggestion> GetSuggestions()
        {
            var result = _userRepository.GetSuggestions();
            var response = _suggestionModelFactory.CreateModelList<Suggestion>(result as List<suggestion>);
            return response as List<Suggestion>;
        }

        public IEnumerable<ServiceLine> GetServiceLines()
        {
            var serviceLines = _serviceLineRepository.GetServiceLines();

            return _serviceLineModelFactory.CreateModelList<ServiceLine>(serviceLines);
        }

        public int UpdateManagersEmail(string usersEmailAddress, string managersEmailAddress, string managersName)
        {
            var user = _userRepository.GetUserByEmailId(usersEmailAddress);

            var request = new ProfileRequest
            {
                UserId = user.ID,
                ProjectCode = user.PROJECT_CODE,
                ProjectName = user.PROJECT_NAME,
                ManagerEmailId = managersEmailAddress,
                ManagerName = managersName,
                SkillSet = new List<Skill>()
            };
            return UpdateProfile(request);

        }

        public string GetNameFromEmailId(string emailId)
        {
            var emailSplit = emailId.Split('@');

            string userName;

            if (emailSplit != null)
            {
                userName = emailSplit[0];

                if (!String.IsNullOrWhiteSpace(userName))
                {
                    var userContext = MapDetailsFromDeloitteNetworkWithoutUserContextObject(userName);

                    return userContext.Name;
                }
                return string.Empty;
            }
            return string.Empty;
        }

        public bool GetTechXAccess()
        {
            if (Convert.ToBoolean(ConfigurationManager.AppSettings[Constants.CheckUsiAccess]) == false)
            {
                return true;
            }

            if (string.IsNullOrEmpty(_userContext?.MsArchiveName))
            {
                throw new InvalidOperationException("User Context has not yet been initiated");
            }

            var archiveName = _userContext.MsArchiveName;

            return Regex.IsMatch(archiveName, @".+(US - )(Hyderabad|Delhi|Bengaluru|Mumbai)[)]");
        }

        public string GetRMGroupEmailAddress(string department)
        {
            var serviceLines = GetServiceLines();

            var currentUsersServiceLine = string.Empty;
            foreach (var serviceLine in serviceLines)
            {
                var splitDep = department.Split(' ');
                if (splitDep.Contains(serviceLine.Name.ToUpperInvariant()) || splitDep.Contains("EBS"))
                {
                    currentUsersServiceLine = serviceLine.Name;
                    break;
                }
            }

            return ConfigurationManager.AppSettings[Constants.RMGroupEmailAddressKeyPrefix + currentUsersServiceLine];
        }
    }
}

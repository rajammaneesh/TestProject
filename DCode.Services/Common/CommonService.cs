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
using Outlook = Microsoft.Office.Interop.Outlook;

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
        private ITaskTypeRepository _taskTypeRepository;
        private ServiceLineModelFactory _serviceLineModelFactory;
        private TaskTypeModelFactory _taskTypeModelFactory;
        private OfferingModelFactory _offeringModelFactory;
        private PortfolioModelFactory _portfolioModelFactory;
        private IOfferingRepository _offeringRepository;
        private IPortfolioRepository _portfolioRepository;
        private IApprovedApplicantRepository _approvedApplicantRepository;
        private ApprovedApplicantModelFactory _approvedApplicantModelFactory;
        private UserPointsModelFactory _userPointsModelFactory;
        private IUserPointsRepository _userPointsRepository;
        private ISubOfferingRepository _subOfferingRepository;
        private SubOfferingModelFactory _subOfferingModelFactory;


        public CommonService(ITaskRepository taskRepository, UserContext userContext, ILogRepository logRepository,
            LogModelFactory logModelFactory, IRequestorRepository requestorRepository, IUserRepository userRepository,
            UserModelFactory userModelFactory, ApplicantSkillModelFactory applicantSkillModelFactory,
            SkillModelFactory skillModelFactory, SuggestionModelFactory suggestionModelFactory,
            IServiceLineRepository serviceLineRepository, ServiceLineModelFactory serviceLineModelFactory,
            ITaskTypeRepository taskTypeRepository, TaskTypeModelFactory taskTypeModelFactory, OfferingModelFactory offeringModelFactory, UserPointsModelFactory userPointsModelFactory,
            ApprovedApplicantModelFactory approvedApplicantModelFactory, PortfolioModelFactory portfolioModelFactory, IOfferingRepository offeringRepository,
            IApprovedApplicantRepository approvedApplicantRepository, IPortfolioRepository portfolioRepository, IUserPointsRepository userPointsRepository, SubOfferingModelFactory subOfferingModelFactory, ISubOfferingRepository subOfferingRepository)
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
            _taskTypeModelFactory = taskTypeModelFactory;
            _taskTypeRepository = taskTypeRepository;
            _offeringModelFactory = offeringModelFactory;
            _portfolioModelFactory = portfolioModelFactory;
            _offeringRepository = offeringRepository;
            _portfolioRepository = portfolioRepository;
            _approvedApplicantRepository = approvedApplicantRepository;
            _approvedApplicantModelFactory = approvedApplicantModelFactory;
            _userPointsModelFactory = userPointsModelFactory;
            _userPointsRepository = userPointsRepository;
            _subOfferingRepository = subOfferingRepository; _approvedApplicantRepository = approvedApplicantRepository;
            _subOfferingModelFactory = subOfferingModelFactory;
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
                        //ErrorSignal.FromCurrentContext().Raise(ex);
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
                        if (propertyName.ToLowerInvariant().Equals(Constants.Mail))
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
                        else if ((propertyName.ToLowerInvariant().Equals(Constants.Location)))
                        {
                            _userContext.LocationName = result.Properties[propertyName][0].ToString();
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
                            var propertyValue = result.Properties[propertyName][0].ToString();

                            if (propertyValue.IndexOf(" ") == -1)
                            {
                                userContext.EmailId = propertyValue + Constants.DeloitteEmailExtn;
                            }
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
            var designation = _userContext.Designation.ToLowerInvariant();

            if (designation.Contains("senior manager") || designation.Contains("specialist leader") || designation.Contains("director") || designation.Contains("partner") || designation.Contains("principal"))
            {
                //_userContext.Role = Enums.Role.Admin;
                _userContext.Role = Role.Requestor;
                _userContext.IsCoreRoleRequestor = true;
            }
            else if (designation.Contains("manager") || designation.Contains("master") || designation.Contains("mngr") || designation.Contains("mgr"))
            {
                _userContext.Role = Role.Requestor;
                _userContext.IsCoreRoleRequestor = true;
            }
            else if (designation.Contains("senior consultant") || designation.Contains("specialist senior") || designation.Contains("asst mgr") || designation.Contains("sr. analyst"))
            {
                _userContext.Role = Role.Requestor;
                _userContext.IsCoreRoleRequestor = true;
            }
            else
            {
                _userContext.Role = Role.Contributor;
                _userContext.IsCoreRoleRequestor = false;
            }

            if (!string.IsNullOrWhiteSpace(_userContext.LocationName))
            {
                _userContext.Location = MapLocation(_userContext.LocationName.ToLowerInvariant());
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
                _userContext.OfferingId = dbUser.OFFERING_ID;
                _userContext.LocationId = dbUser.location_id;
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
                        = dbUser.notification_subscription.First().SUBSCRIPTION_STATUS;
                }
            }
            else
            {
                var userDbObj = _userModelFactory.CreateModel(_userContext);
                var result = _userRepository.InsertUser(userDbObj);
                var dbUserres = _requestorRepository.GetUserByEmailId(_userContext.EmailId);
                _userContext.UserId = dbUserres.ID;
                _userContext.OfferingId = dbUserres.OFFERING_ID;
            }
            ValidateAndSetODCAccess(_userContext);
        }

        private LocationEnum? MapLocation(string locationName)
        {
            switch (locationName)
            {
                case Constants.Hyderabad:
                    return LocationEnum.Hyderabad;
                case Constants.Bengaluru:
                    return LocationEnum.Bengaluru;
                case Constants.Mumbai:
                    return LocationEnum.Mumbai;
                case Constants.Gurgaon:
                    return LocationEnum.Gurgaon;
                default:
                    return null;
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
                    menuItemsList.Add(new MenuItem() { MenuItemName = "CONTACT US", TabName = Constants.ContactUS, NavigationUrl = "/ContactUs/ContactUs", ImageUrlActive = "/Content/Images/Email-Active.png", ImageUrlInactive = "/Content/Images/Email-Inactive.png", CssClass = "contactus-icon" });
                    menuItemsList.Add(new MenuItem() { MenuItemName = "USER MANUAL", TabName = Constants.UserManual, NavigationUrl = "/Content/Documents/TechX%20Demo_v2.pdf", ImageUrlActive = "/Content/Images/download.png", ImageUrlInactive = "/Content/Images/download.png", CssClass = "contactus-icon" }); break;
                case Role.Contributor:
                    menuItemsList.Add(new MenuItem() { MenuItemName = "MY TASKS", TabName = Constants.TabMyTasks, NavigationUrl = "/Contributor/Dashboard", ImageUrlActive = "/Content/Images/dashboard@2x.png", ImageUrlInactive = "/Content/Images/dashboard-disabled@2x.png", CssClass = "mytask-icon" });
                    menuItemsList.Add(new MenuItem() { MenuItemName = "HISTORY", TabName = Constants.TabHistory, NavigationUrl = "/Contributor/History", ImageUrlActive = "/Content/Images/history-active.png", ImageUrlInactive = "/Content/Images/history-icon.png", CssClass = "history-icon" });
                    menuItemsList.Add(new MenuItem() { MenuItemName = "CONTACT US", TabName = Constants.ContactUS, NavigationUrl = "/ContactUs/ContactUs", ImageUrlActive = "/Content/Images/Email-Active.png", ImageUrlInactive = "/Content/Images/Email-Inactive.png", CssClass = "contactus-icon" });
                    menuItemsList.Add(new MenuItem() { MenuItemName = "USER MANUAL", TabName = Constants.UserManual, NavigationUrl = "/Content/Documents/TechX%20Demo_v2.pdf", ImageUrlActive = "/Content/Images/download.png", ImageUrlInactive = "/Content/Images/download.png", CssClass = "contactus-icon" }); break;
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

        public IEnumerable<PortfolioOffering> GetPortfolioOfferings(int taskTypeId)
        {
            var offeringsDisplayList = new List<PortfolioOffering>();
            var portfolios = _portfolioRepository.GetPortfoliosOfferings(taskTypeId);

            foreach (var portfolio in portfolios)
            {
                foreach (var offering in portfolio.offerings)
                {
                    var offeringToDisplay = new PortfolioOffering
                    {
                        PortfolioId = portfolio.Id,
                        OfferingId = offering.Id,
                        OfferingCode = offering.Code,
                        DisplayName = $"{ portfolio.Code} - { offering.Description }"
                    };

                    offeringsDisplayList.Add(offeringToDisplay);
                }
            }

            return offeringsDisplayList;
        }


        public IEnumerable<Offering> GetOfferings()
        {
            var offerings = _offeringRepository.GetOfferings();

            return _offeringModelFactory.CreateModelList<Offering>(offerings);
        }

        public int? GetApprovedApplicantHours()
        {
            var currentUser = GetCurrentUserContext();

            var applicants = _approvedApplicantRepository.GetApprovedApplicants();

            var hoursWorked = _approvedApplicantModelFactory.CreateModelList<ApprovedApplicant>(applicants)
                ?.Where(x => x.APPLICANT_ID == currentUser.UserId);

            var sumOfHoursWorked = hoursWorked.Sum(x => x.HOURS_WORKED);

            return sumOfHoursWorked.HasValue ? Convert.ToInt32(sumOfHoursWorked) : (int?)null;
        }

        //public int? GetUserPoints()
        //{
        //    var currentUser = GetCurrentUserContext();
        //    var users = _userRepository.GetAllUsers();
        //    var userpoints = _userPointsRepository.GetUserPoints();

        //    var offering_Users = _userModelFactory.CreateModelList<user>(users).Where(x => x.OFFERING_ID == currentUser.OfferingId).ToList();
        //    var userPointsList = _userPointsModelFactory.CreateModelList<UserPoints>(userpoints).ToList();

        //    var points = new List<int>();
        //    HashSet<int> userIds = new HashSet<int>(offering_Users.Select(s => s.ID));
        //    var filteredUserPointsList = userPointsList.Where(m => userIds.Contains(m.Id)).ToList();

        //    return filteredUserPointsList.Sum(x => x.points);
        //}

        public int? GetUserPoints()
        {
            var currentUser = GetCurrentUserContext();

            var userpoints = _userPointsRepository.GetUserPointsForUser(
                currentUser.UserId,
                (int)Role.Requestor);

            return userpoints;
        }

        public string GetRequestorEvents()
        {
            var currentUser = GetCurrentUserContext();

            var userpoints = _userPointsRepository
                .GetUserPoints()
                ?.Where(m => m.@event == "REQUESTOR-Task Created"
                     && m.user.OFFERING_ID == currentUser.OfferingId
                     && m.user_role.Id == (int)Role.Requestor)
                ?.GroupBy(x => x.user_id)
                ?.OrderByDescending(x => x.Count())
                ?.Select(x => new
                {
                    UserId = x.Key,
                    TotalRequests = x.Count()
                })
                ?.ToList();

            var benchmarkNumber = Convert.ToInt32(ConfigurationManager.AppSettings["BenchmarkRequests"]);

            var currentUsersRecord = userpoints.Where(x => x.UserId == currentUser.UserId);

            if (userpoints.Any(x => x.TotalRequests >= benchmarkNumber))
            {
                if (currentUsersRecord != null
                    && currentUsersRecord.Any()
                    && currentUsersRecord.First().TotalRequests >= benchmarkNumber)
                {
                    var firstUserPointRecord = userpoints.First();

                    if (firstUserPointRecord.UserId == currentUser.UserId)
                    {
                        return "Congratulations! You are currently the Leading Requestor in ‘your Offering’.";
                    }
                    else
                    {
                        var difference = firstUserPointRecord.TotalRequests - currentUsersRecord.First().TotalRequests;

                        return $"You are now {difference} Requests behind the Requesting Leader in ‘your Offering’.";
                    }
                }
            }
            return "Start creating new Requests now, earn loyalty points and pave your way to the top to be the Requesting Leader in ‘your Offering’.";
        }


        public IEnumerable<Portfolio> GetPortfolios()
        {
            var portfolios = _portfolioRepository.GetPortfolios();

            return _portfolioModelFactory.CreateModelList<Portfolio>(portfolios);
        }

        public IEnumerable<Models.ResponseModels.Common.TaskType> GetTaskTypes()
        {
            var taskTypes = _taskTypeRepository.GetTaskTypes();

            return _taskTypeModelFactory.CreateModelList<Models.ResponseModels.Common.TaskType>(taskTypes);
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
            var offerings = GetOfferings();

            var resourceManagerEmailId = string.Empty;
            foreach (var offering in offerings)
            {
                var splitDep = department.Split(' ');
                if (splitDep.Contains(offering.Code.ToUpperInvariant()))
                {
                    resourceManagerEmailId = offering.RMEmailGroup;
                    break;
                }
            }

            return resourceManagerEmailId;
        }

        public List<string> GetFINotificationRecipientsForOffering(int offeringId)
        {
            var offerings = GetOfferings();

            var matchingOffering = offerings?.FirstOrDefault(x => x.Id == offeringId);

            return matchingOffering?.GetPracticeEmailGroupsAsList();
        }

        public List<string> GetFINotificationRecipientsForSubOffering(int subofferingId)
        {
            var subofferings = _subOfferingRepository.GetSubOfferingById(subofferingId);

            return subofferings?.GetPracticeEmailGroupsAsList();
        }

        public List<string> GetDefaultConsultingMailboxes()
        {
            var offerings = GetOfferings();

            var practiceEmails = offerings
                .Where(x => !string.IsNullOrEmpty(x.PracticeEmailGroup))
                .SelectMany(x => x.GetPracticeEmailGroupsAsList())
                ?.Distinct()
                ?.ToList();

            practiceEmails
                ?.RemoveAll(x => string.IsNullOrEmpty(x?.Trim()));

            return practiceEmails;
        }

        public void MigrateGamificationRecords()
        {
            var offerings = GetOfferings();

            var users = _userRepository.GetAllActiveUsersDetails();

            users.ToList()?.ForEach(x =>
           {
               var userData = GetDesignationAndDepartmentForUser(x.EMAIL_ID);

               if (userData != null)
               {
                   var designation = userData.Item1.ToLowerInvariant();

                   var department = userData.Item2;

                   var offeringId = (int?)null;

                   if (!department.Contains("USI"))
                   {
                       offeringId = null;
                   }

                   try
                   {
                       var departmentCode = department.Substring(0, department.IndexOf("USI"))?.Trim();
                       offeringId = offerings.Where(y => y.Code == departmentCode)?.FirstOrDefault()?.Id;
                   }
                   catch (Exception)
                   {

                   }

                   Role userRole;

                   if (designation.Contains("senior manager") || designation.Contains("specialist leader") || designation.Contains("director") || designation.Contains("partner"))
                   {
                       userRole = Role.Requestor;
                   }
                   else if (designation.Contains("manager") || designation.Contains("master") || designation.Contains("mngr") || designation.Contains("mgr"))
                   {
                       userRole = Role.Requestor;
                   }
                   else if (designation.Contains("senior consultant") || designation.Contains("specialist senior") || designation.Contains("asst mgr") || designation.Contains("sr. analyst"))
                   {
                       userRole = Role.Requestor;
                   }
                   else
                   {
                       userRole = Role.Contributor;
                   }

                   if (userRole == Role.Requestor)
                   {
                       _userPointsRepository.InsertUserPoints(new user_points
                       {
                           created_date = DateTime.Now,
                           points = 5,
                           role_id = (int)Role.Requestor,
                           user_id = x.ID,
                           @event = "REQUESTOR-Existing Requestor",
                       });
                   }

                   _userRepository.UpdateOfferingIdForUser(x.ID, offeringId);

                   _userPointsRepository.InsertUserPoints(new user_points
                   {
                       created_date = DateTime.Now,
                       points = 5,
                       role_id = (int)Role.Contributor,
                       user_id = x.ID,
                       @event = "CONTRIBUTOR-Existing Contributor",
                   });
               }
           });
        }

        public void UpdatingWorkLocationOfExisitingUsers()
        {
            var locations = _userRepository.GetAllLocations();
            int? locationId = null;
            var users = _userRepository.GetAllActiveUsersDetails();

            users.ToList()?.ForEach(x =>
            {
                var userlocation = GetLocationForUser(x.EMAIL_ID);

                if (userlocation != null)
                {
                    try
                    {
                        locationId = locations.Where(y => y.City.ToLower() == userlocation.ToLower())?.FirstOrDefault()?.Id;
                    }
                    catch (Exception)
                    {

                    }

                    _userRepository.UpdateLocationForUser(x.ID, locationId);
                }
            });
        }

        private Tuple<string, string> GetDesignationAndDepartmentForUser(string userName)
        {
            var userNameItem = userName.Split('@')?.First();

            string designation = string.Empty;
            string offering = string.Empty;

            if (string.IsNullOrEmpty(userNameItem))
            {
                return null;
            }

            SearchResultCollection searchResults = null;
            string path = string.Format(ConfigurationManager.AppSettings[Constants.LdapConnection].ToString(), userNameItem);
            using (var directoryEntry = new DirectoryEntry(path))
            using (var directorySearcher = new DirectorySearcher(directoryEntry))
            {
                directorySearcher.Filter = string.Format(Constants.SearchFilter, userNameItem);
                searchResults = directorySearcher.FindAll();

                if (searchResults.Count == 0)
                {
                    return null;
                }

                var propertyNames = searchResults[0].Properties.PropertyNames as List<ResultPropertyCollection>;

                var propertyDescription = new StringBuilder();

                foreach (SearchResult result in searchResults)
                {
                    if (result.Properties != null)
                    {
                        designation = Convert.ToString(result.Properties["title"][0]);
                        offering = Convert.ToString(result.Properties["department"][0]);

                        return Tuple.Create(designation, offering);
                    }
                    //foreach (string propertyName in result.Properties.PropertyNames)
                    //{
                    //    if (propertyName.ToLowerInvariant().Equals(Constants.Title))
                    //    {
                    //        return result.Properties[propertyName][0].ToString();
                    //    }
                    //}
                }
            }
            return null;
        }
        private string GetLocationForUser(string userName)
        {
            var userNameItem = userName.Split('@')?.First();

            string location = string.Empty;

            if (string.IsNullOrEmpty(userNameItem))
            {
                return null;
            }

            SearchResultCollection searchResults = null;
            string path = string.Format(ConfigurationManager.AppSettings[Constants.LdapConnection].ToString(), userNameItem);
            using (var directoryEntry = new DirectoryEntry(path))
            using (var directorySearcher = new DirectorySearcher(directoryEntry))
            {
                directorySearcher.Filter = string.Format(Constants.SearchFilter, userNameItem);
                searchResults = directorySearcher.FindAll();

                if (searchResults.Count == 0)
                {
                    return null;
                }

                var propertyNames = searchResults[0].Properties.PropertyNames as List<ResultPropertyCollection>;

                var propertyDescription = new StringBuilder();

                foreach (SearchResult result in searchResults)
                {
                    if (result.Properties != null)
                    {
                        location = Convert.ToString(result.Properties["physicaldeliveryofficename"][0]);
                        return location;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// This method is responsible to indentify if the user is part of any specific ODC's distribution list.
        /// If yes he will have access to additional feature specific to that ODC
        /// </summary>
        /// <returns></returns>
        private void ValidateAndSetODCAccess(UserContext _userContext)
        {
            _userContext.AccessibleODCId = 0;
            var odcList = ODCReferenceService.GetExistingODCList(AppDomain.CurrentDomain.BaseDirectory + Constants.ODCPath);
            var appOutlook = new Outlook.Application();
            var recepient = appOutlook.Session.CreateRecipient(_userContext.EmailId);
            recepient.Resolve();

            Outlook.AddressEntry addrEntry = recepient.AddressEntry;
            if (addrEntry.Type == "EX" && odcList != null && odcList.ODCList.Any())
            {
                Outlook.ExchangeUser exchUser = addrEntry.GetExchangeUser();
                Outlook.AddressEntries addrEntries = exchUser.GetMemberOfList();
                if (addrEntries != null)
                {
                    foreach (Outlook.AddressEntry exaddrEntry in addrEntries)
                    {
                        var name = exaddrEntry.Name.ToString();
                        foreach (var odc in odcList.ODCList)
                        {
                            if (odc.DistributionList.Split(new char[] { ',' }).Contains(name))
                            {
                                _userContext.AccessibleODCId = Convert.ToInt32(odc.OfferingId);
                                _userContext.HasODCAccess = true;
                                break;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// This method is used to get all the sub offering details by offering Id
        /// </summary>
        /// <param name="offeringId"></param>
        /// <returns></returns>
        public IEnumerable<SubOffering> GetSubOfferings(int offeringId)
        {
            var subofferings = _subOfferingRepository.GetSubOfferings(offeringId);
            return _subOfferingModelFactory.CreateModelList<SubOffering>(subofferings);
        }

    }
}


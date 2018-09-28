using DCode.Common;
using DCode.Data.DbContexts;
using DCode.Data.MetadataRepository;
using DCode.Data.UserRepository;
using DCode.Models.RequestModels;
using DCode.Models.ResponseModels.Common;
using DCode.Models.User;
using DCode.Services.ModelFactory.CommonFactory;
using System;
using System.Collections.Generic;
using System.Linq;
using static DCode.Models.Enums.Enums;

namespace DCode.Services.ModelFactory
{
    public class UserModelFactory : IModelFactory<user>
    {
        private IOfferingRepository _offeringRepository;
        private  IUserRepository _userRepository;

        public UserModelFactory(IOfferingRepository offeringRepository,IUserRepository userRepository)
        {
            _offeringRepository = offeringRepository;
            _userRepository = userRepository;
        }

        public TModel CreateModel<TModel>(user input) where TModel : class
        {
            if (typeof(TModel) == typeof(UserContext))
            {
                return TranslateUser(input) as TModel;
            }
            return null;
        }

        private UserContext TranslateUser(user input)
        {
            var userContext = new UserContext();
            userContext.SkillSet = new List<Skill>();
            //userContext.Designation = input.DESIGNATION;
            //userContext.EmailId = input.EMAIL_ID;
            //userContext.EmployeeId = input.
            userContext.ManagerEmailId = input.MANAGER_EMAIL_ID;
            userContext.ProjectCode = input.PROJECT_CODE;
            userContext.ProjectName = input.PROJECT_NAME;
            userContext.ManagerName = input.PROJECT_MANAGER_NAME;
            foreach (var dbSkill in input.applicantskills)
            {
                var skill = new Skill();
                skill.Id = dbSkill.skill.ID;
                skill.Value = dbSkill.skill.VALUE;
                userContext.SkillSet.Add(skill);
            }
            return userContext;
        }

        public user CreateModel<TModel>(user input, TModel model) where TModel : class
        {
            throw new NotImplementedException();
        }

        public user CreateModel<TModel>(TModel input) where TModel : class
        {
            if (typeof(TModel) == typeof(UserContext))
            {
                return TranslateUserContext(input as UserContext);
            }
            if (typeof(TModel) == typeof(ProfileRequest))
            {
                return TraslateProfile(input as ProfileRequest);
            }
            return null;
        }

        private user TraslateProfile(ProfileRequest profileRequest)
        {
            var user = new user();
            user.ID = profileRequest.UserId;
            user.MANAGER_EMAIL_ID = profileRequest.ManagerEmailId;
            user.PROJECT_MANAGER_NAME = profileRequest.ManagerName;
            user.PROJECT_CODE = profileRequest.ProjectCode;
            user.PROJECT_NAME = profileRequest.ProjectName;

            var subscriptionNotification = new notification_subscription();

            subscriptionNotification.SUBSCRIPTION_STATUS
                = profileRequest.IsSubscribedToNotifications;

            user.notification_subscription.Add(subscriptionNotification);

            return user;
        }

        private user TranslateUserContext(UserContext userContext)
        {
            var dbUser = new user();
            dbUser.ID = userContext.UserId;
            dbUser.DESIGNATION = userContext.Designation;
            dbUser.EMAIL_ID = userContext.EmailId;
            dbUser.FIRST_NAME = userContext.FirstName;
            dbUser.LAST_NAME = userContext.LastName;
            dbUser.OFFERING_ID = GetOfferingFromDepartment(userContext?.DepartmentCode);
            dbUser.location_id = GetLocationIdFromCity(userContext.Location.GetDescription());
            dbUser.STATUS = UserStatus.Active.ToString();
            dbUser.STATUS_DATE = DateTime.Now;

            dbUser.user_points = new List<user_points>();

            if (userContext.Role == Role.Requestor)
            {
                dbUser.user_points.Add(new user_points
                {
                    @event = "REQUESTOR-New Requestor",
                    created_date = DateTime.Now,
                    points = 5,
                    role_id = (int)Role.Requestor,
                    user_id = userContext.UserId
                });
            }

            dbUser.user_points.Add(new user_points
            {
                @event = "CONTRIBUTOR-New Contributor",
                created_date = DateTime.Now,
                points = 5,
                role_id = (int)Role.Contributor,
                user_id = userContext.UserId
            });

            return dbUser;
        }

        private int? GetOfferingFromDepartment(string departmentCode)
        {
            var offerings = _offeringRepository.GetOfferings();

            return offerings?.Where(x => x.Code == departmentCode)?.FirstOrDefault()?.Id;
        }

        private int? GetLocationIdFromCity(string cityName)
        {
            var userlocations = _userRepository.GetAllUser_Locations();

            return userlocations?.Where(x => x.City == cityName)?.FirstOrDefault()?.Id;
        }

        public IEnumerable<TModel> CreateModelList<TModel>(IEnumerable<user> inputList) where TModel : class
        {
            if (typeof(TModel) == typeof(user))
            {
                if (inputList == null)
                {
                    return null;
                }

                var listOfUsers = new List<user>();

                foreach (var item in inputList)
                {
                    listOfUsers.Add(TranslateToOutput(item));
                }

                return listOfUsers as IEnumerable<TModel>;
            }
            return null;
        }

        private user TranslateToOutput(user input)
        {
            var dbUser = new user();
            dbUser.ID = input.ID;
            dbUser.DESIGNATION = input.DESIGNATION;
            dbUser.EMAIL_ID = input.EMAIL_ID;
            dbUser.FIRST_NAME = input.FIRST_NAME;
            dbUser.LAST_NAME = input.LAST_NAME;
            dbUser.OFFERING_ID = input.OFFERING_ID;
            dbUser.STATUS = UserStatus.Active.ToString();
            dbUser.STATUS_DATE = DateTime.Now;
            return dbUser;
        }

    }
}

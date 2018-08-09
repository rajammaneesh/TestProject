using DCode.Data.DbContexts;
using DCode.Models.RequestModels;
using DCode.Models.ResponseModels.Common;
using DCode.Models.User;
using DCode.Services.ModelFactory.CommonFactory;
using System;
using System.Collections.Generic;
using static DCode.Models.Enums.Enums;

namespace DCode.Services.ModelFactory
{
    public class UserModelFactory : IModelFactory<user>
    {
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
            dbUser.STATUS = UserStatus.Active.ToString();
            dbUser.STATUS_DATE = DateTime.Now;
            return dbUser;
        }

        public IEnumerable<TModel> CreateModelList<TModel>(IEnumerable<user> inputList) where TModel : class
        {
            throw new NotImplementedException();
        }
    }
}

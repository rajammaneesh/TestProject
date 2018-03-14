using System.Collections.Generic;
using DCode.Data.DbContexts;
using DCode.Data.Repository;
using System.Linq;
using static DCode.Models.Enums.Enums;
using System.Data.Entity;
using System;

namespace DCode.Data.UserRepository
{
    public class UserRepository : Repository<user>, IUserRepository
    {
        private readonly UserDbContext _context;

        public UserRepository(UserDbContext context)
            : base(context)
        {
            _context = context;
        }

        public IEnumerable<applicantskill> GetSkillsByUserId(int userId)
        {
            IQueryable<applicantskill> query;
            query = Context.Set<applicantskill>().Where(x => x.APPLICANT_ID == userId);
            query.Include(x => x.skill).Load();
            return query.ToList();
        }

        public int InsertUser(user user)
        {
            var result = Context.Set<user>().Add(user);
            return Context.SaveChanges();
        }

        public int UpdateManager(int userId, string managerName, string managerEmailId)
        {
            var result = Context.Set<user>().FirstOrDefault(x => x.ID == userId);
            if (result != null)
            {
                result.PROJECT_MANAGER_NAME = managerName;
                result.MANAGER_EMAIL_ID = managerEmailId;
            }
            return Context.SaveChanges();
        }

        public user GetUserByEmailId(string emailId)
        {
            return Context.Set<user>().Where(x => x.EMAIL_ID == emailId.ToLower()).FirstOrDefault();
        }
        public int InsertTaskApplicant(taskapplicant taskApplicant)
        {
            var result = Context.Set<taskapplicant>().Add(taskApplicant);
            return Context.SaveChanges();
        }
        public int InsertApplicantAndTask(taskapplicant taskApplicant, user applicant)
        {
            var result = Context.Set<user>().Add(applicant);
            var res = Context.Set<taskapplicant>().Add(taskApplicant);
            return Context.SaveChanges();
        }

        public int UpdateProfile(user user, IEnumerable<applicantskill> applicantSkills)
        {
            var currentSkills = Context.Set<applicantskill>()
                .Where(x => x.APPLICANT_ID == user.ID);
            if (currentSkills != null && currentSkills.Count() > 0)
            {
                var res = RemoveApplicantSkills(currentSkills);
            }
            if (applicantSkills != null && applicantSkills.Count() > 0)
            {
                var add = AddApplicantSkills(applicantSkills);
            }
            var dbUsers = Context.Set<user>()
                .Where(x => x.ID == user.ID)
                .Include(x => x.notification_subscription);

            var dbUser = dbUsers.FirstOrDefault();
            dbUser.MANAGER_EMAIL_ID = user.MANAGER_EMAIL_ID;
            dbUser.PROJECT_CODE = user.PROJECT_CODE;
            dbUser.PROJECT_NAME = user.PROJECT_NAME;
            dbUser.PROJECT_MANAGER_NAME = user.PROJECT_MANAGER_NAME;

            if (dbUser.notification_subscription.Any())
            {
                dbUser.notification_subscription.First().subscription_status = user.notification_subscription.First().subscription_status;
            }
            else
            {
                dbUser.notification_subscription = user.notification_subscription;
            }

            //Context.Entry(dbUser).CurrentValues.SetValues(user);
            return Context.SaveChanges();
        }

        public int RemoveApplicantSkills(IEnumerable<applicantskill> applicantSkills)
        {
            var userSkills = Context.Set<applicantskill>().RemoveRange(applicantSkills);
            var remove = Context.SaveChanges();
            return remove;
        }

        public int AddApplicantSkills(IEnumerable<applicantskill> applicantSkills)
        {
            var userSkills = Context.Set<applicantskill>().AddRange(applicantSkills);
            var add = Context.SaveChanges();
            return add;
        }

        public skill GetSkillByName(string skillName)
        {
            var skills = Context.Set<skill>().Where(x => x.VALUE.Equals(skillName) && x.STATUS == SkillStatus.Active.ToString());
            return skills.FirstOrDefault();
        }

        public int AddNewSkill(skill skill)
        {
            var skills = Context.Set<skill>().Add(skill);
            return Context.SaveChanges();
        }

        public int AddSuggestion(suggestion suggestion)
        {
            var suggestions = Context.Set<suggestion>().Add(suggestion);
            return Context.SaveChanges();
        }

        public IEnumerable<suggestion> GetSuggestions()
        {
            return Context.Set<suggestion>().OrderByDescending(x => x.Id).ToList();
        }

        public IEnumerable<string> GetSubscribedUserForTask(string task)
        {
            IQueryable<user> query = Context.Set<user>()
                 .Where(i => (i.notification_subscription.Count > 0 && i.notification_subscription.FirstOrDefault().subscription_status == true)
                    && i.applicantskills.Any(y => y.skill.VALUE.Trim() == task.Trim())
                    && i.STATUS == UserStatus.Active.ToString());

            return query?.Select(i => i.EMAIL_ID)?.ToList();
        }

        public IEnumerable<string> GetAllActiveUsers()
        {
            var query = Context.Set<user>()
                .Where(x => x.STATUS == UserStatus.Active.ToString());

            return query?.Select(x => x.EMAIL_ID)?.ToList();
        }
    }
}

using DCode.Data.DbContexts;
using DCode.Data.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using DCode.Models.Enums;
using System;
using static DCode.Models.Enums.Enums;

namespace DCode.Data.TaskRepository
{
    public class TaskRepository : Repository<task>, ITaskRepository
    {
        private readonly TaskDbContext _context;

        public TaskRepository(TaskDbContext context)
            : base(context)
        {
            _context = context;
        }

        public int InsertTask(task task, IEnumerable<taskskill> taskSkills)
        {
            var insertedTask = Context.Set<task>().Add(task);
            foreach (var skill in taskSkills)
            {
                Context.Set<taskskill>().Add(skill);
            }
            return Context.SaveChanges();
        }

        public IEnumerable<task> GetTaskByEmailId(string emailId)
        {
            var tasks = Context.Set<task>().Where(x => x.user.EMAIL_ID == emailId).ToList();
            return tasks;
        }

        public task GetTaskById(int id)
        {
            IQueryable<task> tasks;
            tasks = Context.Set<task>().Where(x => x.ID == id);
            tasks.Include(x => x.user).Load();
            return tasks.FirstOrDefault();
        }

        public int UpdateTask(task task)
        {
            var dbTasks = Context.Set<task>().Where(x => x.ID == task.ID);
            var dbTask = dbTasks.FirstOrDefault();
            Context.Entry(dbTask).CurrentValues.SetValues(task);
            return Context.SaveChanges();
        }

        public int CloseTask(int taskID)
        {
            var dbTasks = Context.Set<task>().Where(x => x.ID == taskID).FirstOrDefault();
            var dbTask = dbTasks;
            dbTask.STATUS = TaskStatus.Closed.ToString();
            Context.Entry(dbTasks).CurrentValues.SetValues(dbTasks);
            return Context.SaveChanges();
        }

        public IEnumerable<task> GetTasks()
        {
            var tasks = Context.Set<task>().OrderByDescending(x => x.STATUS_DATE);
            return tasks.ToList();
        }

        public int GetTopRatingCountOnEmailId(string emailId)
        {
            var topRatingCount = Context.Set<approvedapplicant>().Where(x => x.user.EMAIL_ID == emailId && x.RATING == Enums.Rating.Good.ToString()).Count();
            return topRatingCount;
        }

        public List<KeyValuePair<string, string>> GetAllCommentsOnEmailId(string emailId)
        {
            List<KeyValuePair<string, string>> commentsbyManagerId = new List<KeyValuePair<string, string>>();
            var tempComments = Context.Set<approvedapplicant>().Where(x => x.user.EMAIL_ID == emailId && x.STATUS == Enums.ApprovedApplicantStatus.Closed.ToString() && x.COMMENTS != null);
            tempComments.Include(x => x.user).Load();

            if (tempComments != null)
            {
                var comments = tempComments.ToList().Select(x => new KeyValuePair<string, string>(x.CREATED_BY, x.COMMENTS));

                foreach (var comment in comments)
                {
                    commentsbyManagerId.Add(comment);
                }
            }

            return commentsbyManagerId;
        }

        public IEnumerable<task> GetTaskHistroyByEmailId(string emailId)
        {
            IQueryable<task> tasks;
            tasks = Context.Set<task>().Where(x => x.user.EMAIL_ID == emailId && x.STATUS == Enums.TaskStatus.Closed.ToString());
            tasks.Include(x => x.taskapplicants.Select(y => y.user)).Load();
            return tasks.ToList();
        }

        public IEnumerable<task> GetTaskHistroyByEmailId(string emailId, int currentPageIndex, int recordsCount, out int totalRecords)
        {
            IQueryable<task> tasks;
            tasks = Context.Set<task>().Where(x => x.user.EMAIL_ID == emailId && x.STATUS == Enums.TaskStatus.Closed.ToString());
            tasks.Include(x => x.taskapplicants.Select(y => y.user)).Load();
            tasks.Include(x => x.taskapplicants.Select(y => y.task.service_line)).Load();
            totalRecords = tasks.Count();
            var filteredRecords = tasks.OrderByDescending(x => x.CREATED_ON).Skip((currentPageIndex - 1) * recordsCount).Take(recordsCount);
            return filteredRecords.ToList();
        }

        public IEnumerable<skill> GetAllSkills()
        {
            return Context.Set<skill>().ToList();
        }

        public IEnumerable<Tuple<string, int>> GetTaskCountBySkillForDate(DateTime date)
        {
            IQueryable<task> tasks = Context.Set<task>()
                            .Where(x => x.CREATED_ON.Value.Day == date.Date.Day
                                && x.CREATED_ON.Value.Month == date.Date.Month
                                && x.CREATED_ON.Value.Year == date.Date.Year
                                && x.STATUS == Enums.TaskStatus.Active.ToString());

            var countResults = tasks
                .GroupBy(x => x.taskskills.FirstOrDefault().skill.VALUE)
                .OrderBy(x => x.Key);

            var mappedResult = new List<Tuple<string, int>>();

            foreach (var result in countResults)
            {
                mappedResult.Add(Tuple.Create<string, int>(result.Key, result.Count()));
            }

            return mappedResult;
        }

        public IEnumerable<task> GetProjectDetailsForNewTasksFromDateForSkill(DateTime date, string skillName)
        {
            IQueryable<task> tasks = Context.Set<task>()
                            .Where(x => x.CREATED_ON.Value.Day == date.Date.Day
                                && x.CREATED_ON.Value.Month == date.Date.Month
                                && x.CREATED_ON.Value.Year == date.Date.Year
                                && x.STATUS == Enums.TaskStatus.Active.ToString()
                                && x.taskskills.Where(y => y.skill.VALUE == skillName).Count() > 0);

            tasks.Include(x => x.taskskills.Select(y => y.skill)).Load();

            return tasks.ToList();
        }
    }
}

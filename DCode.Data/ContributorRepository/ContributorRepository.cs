using DCode.Data.DbContexts;
using DCode.Data.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;
using static DCode.Models.Enums.Enums;

namespace DCode.Data.ContributorRepository
{
    public class ContributorRepository : Repository<Task>, IContributorRepository
    {
        private readonly TaskDbContext _context;

        public ContributorRepository(TaskDbContext context)
            : base(context)
        {
            _context = context;
        }

        public IEnumerable<taskskill> GetTasksBasedOnApplicantSkills(IEnumerable<int> skillIds, int userId)
        {
            IQueryable<taskskill> query;
            query = Context.Set<taskskill>().Where(x => skillIds.Contains(x.SKILL_ID) && x.task.STATUS == Models.Enums.Enums.TaskStatus.Active.ToString()).OrderByDescending(x => x.CREATED_ON);
            query.Include(x => x.skill).Load();
            query.Include(x => x.task.user).Load();
            query.Include(x => x.task.service_line).Load();
            return query.ToList();
        }

        public IEnumerable<taskskill> GetTasksBasedOnSkill(string skill, int currentPageIndex, int recordsCount, out int totalRecords)
        {
            IQueryable<taskskill> query;
            query = Context.Set<taskskill>().Where(x => x.skill.VALUE.Contains(skill) && x.task.STATUS == Models.Enums.Enums.TaskStatus.Active.ToString());
            query.Include(x => x.skill).Load();
            query.Include(x => x.task.user).Load();
            totalRecords = query.Count();
            var filteredRecords = query.OrderByDescending(x => x.CREATED_ON).Skip((currentPageIndex - 1) * recordsCount).Take(recordsCount);
            return filteredRecords.ToList();
        }

        public IEnumerable<task> GetAllTasks(int currentPageIndex, int recordsCount, out int totalRecords)
        {
            IQueryable<task> tasks;
            tasks = Context.Set<task>().Where(x => x.STATUS == Models.Enums.Enums.TaskStatus.Active.ToString());
            tasks.Include(x => x.user).Load();
            tasks.Include(x => x.service_line).Load();
            tasks.Include(x => x.taskskills.Select(y => y.skill)).Load();
            totalRecords = tasks.Count();
            var filteredRecords = tasks.OrderByDescending(x => x.CREATED_ON).Skip((currentPageIndex - 1) * recordsCount).Take(recordsCount);
            return filteredRecords.ToList();
        }

        public int ApplyForTask(taskapplicant taskApplicant)
        {
            var result = Context.Set<taskapplicant>().Add(taskApplicant);
            return Context.SaveChanges();
        }

        public IEnumerable<approvedapplicant> GetTaskStatus(int userId)
        {
            IQueryable<approvedapplicant> query;
            query = Context.Set<approvedapplicant>().Where(x => x.APPLICANT_ID == userId && x.STATUS == ApplicantStatus.Closed.ToString());
            query.Include(x => x.task).Load();
            query.Include(x => x.user).Load();
            return query.ToList();
        }

        public IEnumerable<approvedapplicant> GetTaskHistories(int userId, int currentPageIndex, int recordsCount, out int totalRecords)
        {
            IQueryable<approvedapplicant> query;
            query = Context.Set<approvedapplicant>().Where(x => x.APPLICANT_ID == userId && x.STATUS == ApplicantStatus.Closed.ToString());
            query.Include(x => x.task).Load();
            query.Include(x => x.task.user).Load();
            query.Include(x => x.task.service_line).Load();
            query.Include(x => x.user).Load();
            totalRecords = query.Count();
            var filteredRecords = query.OrderByDescending(x => x.task.CREATED_ON).Skip((currentPageIndex - 1) * recordsCount).Take(recordsCount);
            return filteredRecords.ToList();
        }

        public IEnumerable<approvedapplicant> GetAssignedTask(int userId)
        {
            IQueryable<approvedapplicant> query;
            query = Context.Set<approvedapplicant>().Where(x => x.APPLICANT_ID == userId && x.STATUS == ApprovedApplicantStatus.Active.ToString());
            query.Include(x => x.task).Load();
            query.Include(x => x.user).Load();
            return query.ToList();
        }

        public IEnumerable<approvedapplicant> GetAssignedTask(int userId, int currentPageIndex, int recordsCount, out int totalRecordsCount)
        {
            IQueryable<approvedapplicant> query;
            query = Context.Set<approvedapplicant>().Where(x => x.APPLICANT_ID == userId && x.STATUS == ApprovedApplicantStatus.Active.ToString());
            query.Include(x => x.task).Load();
            query.Include(x => x.task.user).Load();
            query.Include(x => x.task.service_line).Load();
            query.Include(x => x.user).Load();
            totalRecordsCount = query.Count();
            var filteredRecords = query.OrderByDescending(x => x.task.CREATED_ON).Skip((currentPageIndex - 1) * recordsCount).Take(recordsCount);
            return filteredRecords.ToList();
        }

        public IEnumerable<taskapplicant> GetAppliedTasks(int userId)
        {
            IQueryable<taskapplicant> query;
            query = Context.Set<taskapplicant>().Where(x => x.APPLICANT_ID == userId && x.STATUS == TaskApplicant.Active.ToString());
            return query.ToList();
        }

        public int UpdateHours(int approvedApplicantId, int hours)
        {
            var approvedApplicant = Context.Set<approvedapplicant>().Where(x => x.ID == approvedApplicantId).FirstOrDefault();
            if (approvedApplicant != null)
            {
                if (approvedApplicant.HOURS_WORKED != null)
                {
                    approvedApplicant.HOURS_WORKED += hours;
                }
                else
                {
                    approvedApplicant.HOURS_WORKED = hours;
                }
            }
            return Context.SaveChanges();
        }

        public IEnumerable<taskskill> GetFilteredTasks(List<string> skillFilters,
            string serviceLine,
            int selectedTaskType,
            string searchText,
            int currentPageIndex,
            int recordsCount,
            out int totalRecords)
        {
            IQueryable<taskskill> query = null;

            query = Context.Set<taskskill>()
                .Where(x => x.task.STATUS == Models.Enums.Enums.TaskStatus.Active.ToString());

            if (skillFilters != null && skillFilters.Any())
            {
                query = query.Where(x => skillFilters.Contains(x.skill.VALUE));
            }

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                query = query.Where(x => x.skill.VALUE.Contains(searchText)
                             || x.task.PROJECT_NAME.Contains(searchText));
            }

            if (!string.IsNullOrEmpty(serviceLine))
            {
                query = query.Where(x => x.task.service_line.Name.ToString().Equals(serviceLine));
            }

            query = query.Where(x => x.task.TASK_TYPE_ID == selectedTaskType);

            query.Include(x => x.skill).Load();

            query.Include(x => x.task.user).Load();

            query.Include(x => x.task.service_line).Load();

            totalRecords = query.Count();

            var filteredRecords = query.OrderByDescending(x => x.CREATED_ON).Skip((currentPageIndex - 1) * recordsCount).Take(recordsCount);

            return filteredRecords.ToList();
        }
    }
}

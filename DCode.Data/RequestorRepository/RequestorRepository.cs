using DCode.Data.DbContexts;
using DCode.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using DCode.Common;
using System.Linq.Dynamic;
using static DCode.Models.Enums.Enums;

namespace DCode.Data.RequestorRepository
{
    public class RequestorRepository : Repository<task>, IRequestorRepository
    {
        private readonly TaskDbContext _context;

        public RequestorRepository(TaskDbContext context)
            : base(context)
        {
            _context = context;
        }

        /// <summary>
        /// Returns tasks that are pending for approval. Results will be returned based on the page index and total records per page
        /// </summary>
        /// <param name="currentPageIndex"></param>
        /// <param name="recordsCount"></param>
        /// <param name="emailId"></param>
        /// <returns></returns>
        public IEnumerable<task> GetTaskApplicantsForApproval(int selectedTaskTypeId, int currentPageIndex, int recordsCount, string emailId, out int totalRecords)
        {
            var start = DateTime.Now;
            IQueryable<task> tasks;
            //Admin login
            if (string.IsNullOrEmpty(emailId))
            {
                tasks = Context.Set<task>();
            }
            else
            {
                tasks = Context.Set<task>().Where(x => x.user.EMAIL_ID == emailId
                && x.STATUS == TaskStatus.Active.ToString()
                && x.TASK_TYPE_ID == selectedTaskTypeId);
            }
            tasks.Include(x => x.taskskills.Select(y => y.skill)).Load();
            tasks.Include(x => x.user).Load();
            tasks.Include(x => x.offering).Load();
            tasks.Include(x => x.taskapplicants.Select(y => y.user)).Load();
            totalRecords = tasks.Count();
            //Pick records based on the pageindex.
            var filteredRecords = tasks.OrderByDescending(x => x.CREATED_ON).Skip((currentPageIndex - 1) * recordsCount).Take(recordsCount);
            var end = DateTime.Now - start;
            return filteredRecords.ToList();
        }

        /// <summary>
        /// Assigns Task to the applicant and returns integer if successfull
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="applicantId"></param>
        /// <returns></returns>
        public int AssignTask(approvedapplicant dbApprovedApplicant)
        {
            var dbTask = Context.Set<task>().Where(x => x.ID == dbApprovedApplicant.TASK_ID).FirstOrDefault();
            var dbApplicant = Context.Set<taskapplicant>().Where(x => x.ID == dbApprovedApplicant.ID).FirstOrDefault();
            dbTask.STATUS = TaskStatus.Assigned.ToString();
            dbApplicant.STATUS = ApplicantStatus.Assigned.ToString();
            dbApprovedApplicant.STATUS = ApplicantStatus.Active.ToString();

            Context.Set<approvedapplicant>().Add(dbApprovedApplicant);
            var result = Context.SaveChanges();
            return result;
        }

        //public IEnumerable<approvedapplicant> GetApprovedApplicantsByTaskId(int taskId)
        //{
        //    var applicants = Context.Set<task>().SelectMany(x => x.applicants).Where(y => y.TASK_ID == taskId);
        //    var approvedApplicants = applicants.SelectMany(z => z.approvedapplicants);
        //    return approvedApplicants;
        //}

        public IEnumerable<approvedapplicant> GetStatusOftasks(int selectedTaskType, string emailId, int currentPageIndex, int recordsCount, TaskStatusSortFields sortField, SortOrder sortOrder, out int totalRecords)
        {
            IEnumerable<int> taskIdList;
            //Admin login
            //if (string.IsNullOrEmpty(emailId))
            //{
            //    taskIdList = Context.Set<task>().Where(x => x.STATUS != Enums.TaskStatus.Closed.ToString()).Select(y => y.ID).ToList();
            //}
            //else
            //{
            taskIdList = Context.Set<task>().Where(x => x.user.EMAIL_ID == emailId
            && x.STATUS == TaskStatus.Assigned.ToString()
            && x.TASK_TYPE_ID == selectedTaskType).Select(y => y.ID).ToList();
            //}

            IQueryable<approvedapplicant> query;
            query = Context.Set<approvedapplicant>().Where(x => taskIdList.Contains(x.TASK_ID));
            query.Include(x => x.user).Load();
            query.Include(x => x.task).Load();
            query.Include(x => x.task.offering).Load();
            query.Include(x => x.task.service_line).Load();
            totalRecords = query.Count();

            ////Pick records based on the pageindex.
            if (sortField == TaskStatusSortFields.Name)
            {
                query = sortOrder == SortOrder.ASC ?
                    query.OrderBy(x => x.user.FIRST_NAME).Skip((currentPageIndex - 1) * recordsCount).Take(recordsCount) :
                    query.OrderByDescending(x => x.user.FIRST_NAME).Skip((currentPageIndex - 1) * recordsCount).Take(recordsCount);
            }
            else if (sortField == TaskStatusSortFields.DueDate)
            {
                query = sortOrder == SortOrder.ASC ?
                    query.OrderBy(x => x.task.DUE_DATE).Skip((currentPageIndex - 1) * recordsCount).Take(recordsCount) :
                    query.OrderByDescending(x => x.task.DUE_DATE).Skip((currentPageIndex - 1) * recordsCount).Take(recordsCount);
            }
            else if (sortField == TaskStatusSortFields.Hours)
            {
                query = sortOrder == SortOrder.ASC ?
                    query.OrderBy(x => x.task.HOURS).Skip((currentPageIndex - 1) * recordsCount).Take(recordsCount) :
                    query.OrderByDescending(x => x.task.HOURS).Skip((currentPageIndex - 1) * recordsCount).Take(recordsCount);
            }
            else if (sortField == TaskStatusSortFields.ProjectName)
            {
                query = sortOrder == SortOrder.ASC ?
                    query.OrderBy(x => x.task.PROJECT_NAME).Skip((currentPageIndex - 1) * recordsCount).Take(recordsCount) :
                    query.OrderByDescending(x => x.task.PROJECT_NAME).Skip((currentPageIndex - 1) * recordsCount).Take(recordsCount);
            }
            else if (sortField == TaskStatusSortFields.TaskName)
            {
                query = sortOrder == SortOrder.ASC ?
                    query.OrderBy(x => x.task.TASK_NAME).Skip((currentPageIndex - 1) * recordsCount).Take(recordsCount) :
                    query.OrderByDescending(x => x.task.TASK_NAME).Skip((currentPageIndex - 1) * recordsCount).Take(recordsCount);
            }
            return query.ToList();
        }

        //public approvedapplicant GetApprovedApplicantById(int id)
        //{
        //    return Context.Set<approvedapplicant>().Where(x => x.ID == id).FirstOrDefault();
        //}

        public int ReviewTask(approvedapplicant approvedApplicant)
        {
            //TBD - populate Update fields
            var dbApprovedApplicant = Context.Set<approvedapplicant>().Where(x => x.ID == approvedApplicant.ID).FirstOrDefault();
            dbApprovedApplicant.RATING = approvedApplicant.RATING;
            dbApprovedApplicant.WORK_AGAIN = approvedApplicant.WORK_AGAIN;
            dbApprovedApplicant.COMMENTS = approvedApplicant.COMMENTS;
            dbApprovedApplicant.STATUS = ApprovedApplicantStatus.Closed.ToString();

            //var applicant = Context.Set<taskapplicant>().Where(x => x.ID == dbApprovedApplicant.ID).FirstOrDefault();
            IQueryable<taskapplicant> query;
            query = Context.Set<taskapplicant>().Where(x => x.TASK_ID == dbApprovedApplicant.TASK_ID && x.STATUS == TaskApplicant.Assigned.ToString());
            query.Include(x => x.task).Load();
            var applicant = query.FirstOrDefault();
            applicant.task.STATUS = TaskStatus.Closed.ToString();
            applicant.STATUS = ApplicantStatus.Closed.ToString();
            //var applicant = Context.Set<>().Where(x => x.ID == dbApprovedApplicant.APPLICANT_ID).FirstOrDefault();
            //var task = Context.Set<task>().Where(x => x.ID == applicant.TASK_ID).FirstOrDefault();
            //applicant.STATUS = Enums.ApplicantStatus.Closed.ToString();
            //task.STATUS = Enums.TaskStatus.Closed.ToString();
            var count = Context.SaveChanges();
            return count;
        }

        public approvedapplicant GetApprovedApplicant(int approvedAppId)
        {
            IQueryable<approvedapplicant> query;
            query = Context.Set<approvedapplicant>().Where(x => x.TASK_ID == approvedAppId);
            query.Include(x => x.task).Load();
            query.Include(x => x.user).Load();
            return query.FirstOrDefault();
        }


        public IEnumerable<taskapplicant> GetTaskApplicantsForPermissions(int currentPageIndex, int recordsCount, string emailId, out int totalRecords)
        {
            var start = DateTime.Now;
            IQueryable<taskapplicant> taskapplicant;
            //Admin login
            if (string.IsNullOrEmpty(emailId))
            {
                taskapplicant = Context.Set<taskapplicant>();
            }
            else
            {
                taskapplicant = Context.Set<taskapplicant>().Where(x => x.user.MANAGER_EMAIL_ID == emailId && x.STATUS == TaskStatus.Active.ToString());
            }
            taskapplicant.Include(x => x.task).Load();
            taskapplicant.Include(x => x.task.offering).Load();
            taskapplicant.Include(x => x.user).Load();
            totalRecords = taskapplicant.Count();
            //query = tasks;
            //Pick records based on the pageindex.
            var filteredRecords = taskapplicant.OrderByDescending(x => x.CREATED_ON).Skip((currentPageIndex - 1) * recordsCount).Take(recordsCount);
            filteredRecords.ToList();
            var end = DateTime.Now - start;
            return filteredRecords.ToList();
        }

        public int GetPermissionsCount(string emailId)
        {
            var applicantList = Context.Set<taskapplicant>().Where(x => x.user.MANAGER_EMAIL_ID == emailId && x.STATUS == TaskStatus.Active.ToString());
            return applicantList.Count();
        }

        public int AllowTask(taskapplicant taskApplicant)
        {
            var dbApplicant = Context.Set<taskapplicant>().Where(x => x.ID == taskApplicant.ID).FirstOrDefault();
            dbApplicant.STATUS = ApplicantStatus.ManagerApproved.ToString();
            var result = Context.SaveChanges();
            return result;
        }

        public int RejectTask(taskapplicant taskApplicant)
        {
            var dbApplicant = Context.Set<taskapplicant>().Where(x => x.ID == taskApplicant.ID).FirstOrDefault();
            dbApplicant.STATUS = ApplicantStatus.ManagerRejected.ToString();
            var count = Context.SaveChanges();
            return count;
        }

        public user GetUserByEmailId(string emailId)
        {
            IQueryable<user> query;
            query = Context.Set<user>().Where(x => x.EMAIL_ID == emailId);
            query.Include(x => x.applicantskills.Select(y => y.skill)).Load();
            query.Include(x => x.notification_subscription).Load();
            return query.ToList().FirstOrDefault();
        }

        public IEnumerable<skill> SearchSkill(string searchParam)
        {
            var searchParamLower = searchParam.ToLowerInvariant();
            IQueryable<skill> query;
            query = Context.Set<skill>().Where(x => x.VALUE.Contains(searchParamLower));
            return query.ToList();
        }

        public taskapplicant GetTaskApplicantByApplicantId(int taskApplicantId)
        {
            IQueryable<taskapplicant> query;
            query = Context.Set<taskapplicant>().Where(x => x.ID == taskApplicantId);
            query.Include(x => x.task).Load();
            query.Include(x => x.user).Load();
            return query.FirstOrDefault();
        }
    }
}

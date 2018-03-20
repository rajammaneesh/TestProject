using DCode.Models.RequestModels;
using DCode.Models.ResponseModels.Requestor;
using DCode.Services.Common;
using DCode.Services.Requestor;
using DCode.Web.Security;
using System.Web.Mvc;
using static DCode.Models.Enums.Enums;

namespace DCode.Web.Controllers
{
    [AuthorizeDCode]
    public class RequestorController : Controller
    {
        private IRequestorService _requestorService;
        private ICommonService _commonService;
        public RequestorController(IRequestorService requestorService, ICommonService commonService)
        {
            _requestorService = requestorService;
            _commonService = commonService;
        }

        [AuthorizeRoute(Role.Requestor)]
        public ActionResult Requestor()
        {
            return View();
        }

        [AuthorizeRoute(Role.Requestor)]
        public ActionResult Dashboard()
        {
            return View();
        }

        [AuthorizeRoute(Role.Requestor)]
        public ActionResult NewTasks()
        {
            return View();
        }
        [AuthorizeRoute(Role.Requestor)]
        public ActionResult Permissions()
        {
            return View();
        }

        [AuthorizeRoute(Role.Requestor)]
        public ActionResult History()
        {
            return View();
        }

        [HttpGet]
        /// <summary>
        /// Method should return the requestor tasks that are pending his approval. This would return records based on the inputs(Pagination format)
        /// Return records in Descending order of task creation date
        /// </summary>
        /// <param name="currentPageIndex"></param>
        /// <param name="recordsCount"></param>
        /// <returns></returns>
        public JsonResult GetTaskApplicantsForApproval(int selectedTaskTypeId, int currentPageIndex = 1, int recordsCount = 10)
        {
            var tasks = _requestorService.GetTaskApplicantsForApproval(selectedTaskTypeId, currentPageIndex, recordsCount);
            return Json(tasks, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Returns all the requestor tasks statuses in pagination format. Sort order can be requested in parameters 
        /// </summary>
        /// <param name="currentPageIndex"></param>
        /// <param name="recordsCount"></param>
        /// <param name="sortField"></param>
        /// <param name="sortOrder"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetStatusOftasks(int selectedTaskType, int currentPageIndex = 1, int recordsCount = 10, TaskStatusSortFields sortField = TaskStatusSortFields.Name, SortOrder sortOrder = SortOrder.DESC)
        {
            var tasksStatuses = _requestorService.GetStatusOftasks(selectedTaskType, currentPageIndex, recordsCount, sortField, sortOrder);
            return Json(tasksStatuses, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ReviewTask(ReviewTaskRequest taskRequest)
        {
            var result = _requestorService.ReviewTask(taskRequest);

            return Json(result, JsonRequestBehavior.DenyGet);
        }

        /// <summary>
        /// Assigns a particular task to an Applicant
        /// </summary>
        /// <param name="assignTaskRequest"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AssignTask(AssignTaskRequest assignTaskRequest)
        {
            var result = _requestorService.AssignTask(assignTaskRequest);

            return Json(result, JsonRequestBehavior.DenyGet);
        }

        //[HttpGet]
        //public JsonResult GetTaskhistory()
        //{
        //    return Json(_requestorService.GetTaskHistory(), JsonRequestBehavior.AllowGet);
        //}

        [HttpGet]
        public JsonResult GetTaskhistory(int currentPageIndex, int recordsCount)
        {
            return Json(_requestorService.GetTaskHistories(currentPageIndex, recordsCount), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        ///<summary>
        ///Method should return the requestor tasks that are pending his approval. This would return records based on the inputs(Pagination format)
        ///Return records in Descending order of task creation date
        ///</summary>
        ///<param name="currentPageIndex"></param>
        ///<param name="recordsCount"></param>
        ///<returns></returns>
        public JsonResult GetTaskApplicantsForPermissions(int currentPageIndex = 1, int recordsCount = 10)
        {
            var tasks = _requestorService.GetTaskApplicantsForPermissions(currentPageIndex, recordsCount);
            return Json(tasks, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AllowTask(ApproveTaskRequest allowTaskRequest)
        {
            var result = _requestorService.AllowTask(allowTaskRequest);
            return Json(result, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        public JsonResult RejectTask(RejectTaskRequest rejecttaskRequest)
        {
            var result = _requestorService.RejectTask(rejecttaskRequest);
            return Json(result, JsonRequestBehavior.DenyGet);
        }

        public JsonResult IsFirstTimeForTaskHistory()
        {
            return Json(false, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult IsFirstTimeUserForNewTask()
        {
            return Json(_requestorService.IsFirstTimeUserForNewTask(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetPermissionsCount()
        {
            return Json(_requestorService.GetPermissionsCount(), JsonRequestBehavior.AllowGet);
        }
    }
}
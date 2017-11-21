using DCode.Common;
using DCode.Services.Contributor;
using DCode.Web.Security;
using System.Web.Mvc;
using static DCode.Models.Enums.Enums;

namespace DCode.Web.Controllers
{
    [AuthorizeDCode]
    public class ContributorController : Controller
    {
        private IContributorService _contributorService;
        public ContributorController(IContributorService contributorService)
        {
            _contributorService = contributorService;
        }

        [AuthorizeRoute(Role.Contributor)]
        public ActionResult Dashboard()
        {
            return View();
        }

        [AuthorizeRoute(Role.Contributor)]
        public ActionResult Permissions()
        {
            return View();
        }

        [AuthorizeRoute(Role.Contributor)]
        public ActionResult History()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetTaskHistory()
        {
            return Json(_contributorService.GetTaskHistory(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetTaskHistory(int currentPageIndex, int totalRecords)
        {
            return Json(_contributorService.GetTaskHistory(), JsonRequestBehavior.AllowGet);
        }

        [AuthorizeRoute(Role.Contributor)]
        public JsonResult GetTasks()
        {
            return Json(_contributorService.GetTasksBasedOnApplicantSkills(), JsonRequestBehavior.AllowGet);
        }


        [AuthorizeRoute(Role.Contributor)]
        public JsonResult ApplyTask(int taskId, string emailAddress, string statementOfPurpose)
        {
            var result = _contributorService.ApplyTask(taskId, emailAddress, statementOfPurpose);

            return Json(result, JsonRequestBehavior.DenyGet);
        }

        [AuthorizeRoute(Role.Contributor)]
        public JsonResult GetAssignedTasks(int currentPageIndex, int recordsCount)
        {
            return Json(_contributorService.GetApprovedTasksForCurrentUser(currentPageIndex, recordsCount), JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateHours(int approvedApplicantId, int hours)
        {
            return Json(_contributorService.UpdateHours(approvedApplicantId, hours), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllTasks(string searchKey, int currentPageIndex, int recordsCount, string searchFilter)
        {
            return Json(_contributorService.GetAllTasks(searchKey, currentPageIndex, recordsCount, searchFilter), JsonRequestBehavior.DenyGet);
        }

        public JsonResult GetTaskHistories(int currentPageIndex, int recordsCount)
        {
            return Json(_contributorService.GetTaskHistories(currentPageIndex, recordsCount), JsonRequestBehavior.AllowGet);
        }
    }
}
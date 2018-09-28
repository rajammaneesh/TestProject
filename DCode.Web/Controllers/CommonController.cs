using DCode.Common;
using DCode.Models.RequestModels;
using DCode.Models.User;
using DCode.Services.Common;
using DCode.Services.Reporting;
using DCode.Web.Models;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using static DCode.Models.Enums.Enums;

namespace DCode.Web.Controllers
{
    public class CommonController : Controller
    {
        private ICommonService _commonService;

        private IReportingService _reportingService;


        public CommonController(ICommonService commonService, IReportingService reportingService)
        {
            _commonService = commonService;
            _reportingService = reportingService;
        }
        // GET: Common
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetRecords()
        {
            var userContext = _commonService.GetCurrentUserContext();

            var internalUsers = ConfigurationManager.AppSettings["InternalAdminUsers"];

            if (internalUsers != null
                && internalUsers.Split(',').Any(x => x == userContext.EmailId))
            {
                if (TempData.ContainsKey("RecordsModel"))
                {
                    return View(TempData["RecordsModel"]);
                }

                return View();
            }

            TempData[Constants.ErrorRedirectType] = ErrorRedirectType.Unauthorized;

            return RedirectToAction("Index", "Error");
        }

        [HttpPost]
        public ActionResult FetchDbRecords(DbRecordsViewModel model, string refresh, string get)
        {
            if (refresh != null
                || string.IsNullOrEmpty(model?.Query))
            {
                model.Query = null;
            }
            else if (model.Query.Trim().StartsWith("insert", System.StringComparison.CurrentCultureIgnoreCase)
                || model.Query.Trim().StartsWith("delete", System.StringComparison.CurrentCultureIgnoreCase))
            {
                model.Query = null;
            }
            else if (get != null)
            {
                var result = _reportingService.ExecuteDbQuery(model.Query);

                model.Records = result;
            }

            TempData["RecordsModel"] = model;

            return RedirectToAction("GetRecords");
        }

        public ActionResult User()
        {
            return View();
        }

        public JsonResult GetCurrentUserContext()
        {
            var userContext = _commonService.GetCurrentUserContext();
            return Json(userContext, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SwitchRole()
        {
            var user = _commonService.SwitchRole();
            return Json(user, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SwitchRoleFromLayout(string roleFromUI)
        {
            var user = _commonService.SwitchRole(roleFromUI);
            return Json(user, JsonRequestBehavior.DenyGet);
        }

        public void TestElmah()
        {
            var i = 0;
            var j = 2 / i;
        }

        public JsonResult GetDbLogs()
        {
            return Json(_commonService.GetDBLogs(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Common()
        {
            return View();
        }

        public JsonResult SetUser(string firstName, string lastName, string emailId, string role, string managerEmailId, string department)
        {
            var user = new UserContext { FirstName = firstName, LastName = lastName, EmailId = emailId, Role = role == "Contributor" ? Role.Contributor : Role.Requestor, Designation = role == "Contributor" ? "Consultant" : "Manager", ManagerEmailId = managerEmailId, Department = department, MsArchiveName = "Personal Archive - Lastname, Test (US - Hyderabad)" };
            //user = _commonService.GetCurrentUserContext();
            SessionHelper.Save(Constants.MockUser, user);
            return Json("success", JsonRequestBehavior.DenyGet);
        }

        public JsonResult Apply(int taskId, string firstName, string lastName, string emailId, string mgrEmail)
        {
            var app = new ApplicantRequest() { TaskId = taskId, FirstName = firstName, LastName = lastName, EmailId = emailId, ManagerEmailId = mgrEmail };
            var result = _commonService.InsertApplicant(app);
            return Json(result, JsonRequestBehavior.DenyGet);
        }

        public JsonResult SearchSkill(string searchParam)
        {
            var results = _commonService.SearchSkill(searchParam);
            return Json(results, JsonRequestBehavior.AllowGet);
        }

        public JsonResult testemail()
        {
            EmailHelper.AssignNotification("User", "sample task", "project", "WBC-C_O_D_E", "mrajam@deloitte.com", "risen@deloitte.com", "Test Offering");
            EmailHelper.ReviewNotification("User", "task", "projectname", "risen@deloitte.com", "risen@deloitte.com", "test Offering");
            EmailHelper.SendApproveRejectNotification("User", "task", "projectname", EmailType.Approved, "risen@deloitte.com", "risen@deloitte.com", "Test Offering");
            EmailHelper.SendApproveRejectNotification("User", "task", "projectname", EmailType.Rejected, "risen@deloitte.com", "risen@deloitte.com", "Test Offering");
            return Json("succes", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult InsertNewSkill(string skillValue)
        {
            var result = _commonService.InsertNewSkill(skillValue);
            return Json(result, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        public JsonResult InsertNewSuggestion(string suggestion)
        {
            var result = _commonService.InsertNewSuggestion(suggestion);
            return Json(result, JsonRequestBehavior.DenyGet);
        }

        [HttpGet]
        public JsonResult GetSuggestions()
        {
            return Json(_commonService.GetSuggestions(), JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetServiceLines()
        {
            return Json(_commonService.GetServiceLines(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetPortfolioOfferings(int taskTypeId)
        {
            return Json(_commonService.GetPortfolioOfferings(taskTypeId), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetOfferings()
        {
            return Json(_commonService.GetOfferings(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetPortfolios()
        {
            return Json(_commonService.GetPortfolios(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetTaskTypes()
        {
            return Json(_commonService.GetTaskTypes(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetNameFromEmailId(string emailId)
        {
            return Json(_commonService.GetNameFromEmailId(emailId));
        }


        [HttpGet]
        public JsonResult StartGamificationMigration()
        {
            _commonService.MigrateGamificationRecords();

            return Json("done");
        }

        [HttpGet]
        public JsonResult GetBannerMessage()
        {
            var currentUser = _commonService.GetCurrentUserContext();

            if (currentUser.Role == Role.Requestor)
            {
                //  var message = _commonService.GetRequestorEvents();
                var message = Constants.RequestorGamificationMessage;

                return Json(message, JsonRequestBehavior.AllowGet);
            }

            return Json(null, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetGamificationStats()
        {
            var currentUser = _commonService.GetCurrentUserContext();

            if (currentUser.Role == Role.Contributor)
            {
                var hours = _commonService.GetApprovedApplicantHours();

                return Json($"{ hours ?? 0} hours", JsonRequestBehavior.AllowGet);
            }
            else
            {
                var points = _commonService.GetUserPoints();

                return Json($"{ points ?? 0} points", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult SetModalLoaded()
        {
            var isModalLoaded = SessionHelper.Retrieve("setModalLoaded");

            if (isModalLoaded == null)
            {
                SessionHelper.Save("setModalLoaded", true);
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}
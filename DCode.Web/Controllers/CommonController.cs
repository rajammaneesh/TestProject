using DCode.Common;
using DCode.Models.RequestModels;
using DCode.Models.User;
using DCode.Services.Common;
using System.Web.Mvc;
using static DCode.Models.Enums.Enums;

namespace DCode.Web.Controllers
{
    public class CommonController : Controller
    {
        private ICommonService _commonService;
        public CommonController(ICommonService commonService)
        {
            _commonService = commonService;
        }
        // GET: Common
        public ActionResult Index()
        {
            return View();
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
            EmailHelper.AssignNotification("User", "sample task", "project", "WBC-C_O_D_E", "mrajam@deloitte.com", "risen@deloitte.com");
            EmailHelper.ReviewNotification("User", "task", "projectname", "risen@deloitte.com", "risen@deloitte.com");
            EmailHelper.SendApproveRejectNotification("User", "task", "projectname", EmailType.Approved, "risen@deloitte.com", "risen@deloitte.com");
            EmailHelper.SendApproveRejectNotification("User", "task", "projectname", EmailType.Rejected, "risen@deloitte.com", "risen@deloitte.com");
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
        public JsonResult GetNameFromEmailId(string emailId)
        {
            return Json(_commonService.GetNameFromEmailId(emailId));
        }
    }
}
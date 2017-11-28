using DCode.Common;
using DCode.Services.Common;
using DCode.Web.Security;
using System;
using System.Configuration;
using System.Web.Mvc;
using static DCode.Models.Enums.Enums;

namespace DCode.Web.Controllers
{
    public class HomeController : Controller
    {
        private ICommonService _commonService;
        public HomeController(ICommonService commonService)
        {
            _commonService = commonService;
        }

        public ActionResult Index()
        {
            if (SessionHelper.Retrieve(Constants.MockUser) == null && ConfigurationManager.AppSettings[Constants.EnableTestFlow].ToString().Equals(Constants.True))
            {
                return View();
            }
            else
            {
                var auth = new AuthorizeDCode();
                auth.OnAuthorization(new AuthorizationContext());

                var userContext = _commonService.GetCurrentUserContext();

                if (userContext == null
                    || Convert.ToString(ConfigurationManager.AppSettings[Constants.GenerateRedirectToError]) == "true")
                {
                    TempData[Constants.ErrorRedirectType]
                       = ErrorRedirectType.NonUsiPractitioner;

                    return RedirectToAction("Index", "Error");
                }


                if (userContext.Role == Role.Requestor)
                {
                    return RedirectToAction("newtasks", "requestor");
                }
                else
                {
                    if (userContext.SkillSet == null || (userContext.SkillSet !=null && userContext.SkillSet.Count == 0))
                    {
                        return RedirectToAction("profile", "profile");
                    }
                    return RedirectToAction("dashboard", "contributor");
                }
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
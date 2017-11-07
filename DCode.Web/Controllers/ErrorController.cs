using DCode.Common;
using DCode.Web.Models;
using DCode.Web.Security;
using System.Web.Mvc;
using static DCode.Common.Enums;

namespace DCode.Web.Controllers
{
    [AuthorizeDCode]
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult Index()
        {
            var redirectType = (ErrorRedirectType)TempData[Constants.ErrorRedirectType];

            return View(new ErrorModule
            {
                Message = GetErrorMessageForRedirectType(ErrorRedirectType.NonUsiPractitioner)
            });
        }

        private string GetErrorMessageForRedirectType(ErrorRedirectType? errorType)
        {
            if (errorType == null
                || !errorType.HasValue)
            {
                return "default message";
            }

            return errorType.Value.GetDescription();
        }
    }
}
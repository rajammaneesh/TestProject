using DCode.Common;
using DCode.Web.Models;
using System.Web.Mvc;
using static DCode.Common.Enums;

namespace DCode.Web.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult Index()
        {

            var redirectType = TempData[Constants.ErrorRedirectType] != null
                ? (ErrorRedirectType)TempData[Constants.ErrorRedirectType]
                : ErrorRedirectType.Unknown;

            return View(new ErrorModule
            {
                Message = redirectType.GetDescription()
            });
        }
    }
}
using DCode.Models.RequestModels;
using DCode.Services.Common;
using System.Web.Mvc;

namespace DCode.Web.Controllers
{
    //[AuthorizeDCode]
    public class ProfileController : Controller
    {
        private ICommonService _commonService;
        public ProfileController(ICommonService commonService)
        {
            _commonService = commonService;
        }
        public ActionResult Profile()
        {
            return View();
        }

        [HttpPost]
        public JsonResult UpdateProfile(ProfileRequest profileRequest)
        {
            var result = _commonService.UpdateProfile(profileRequest);

            return Json(result, JsonRequestBehavior.DenyGet);
        }
    }
}
using DCode.Common;
using DCode.Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DCode.Services.Security
{
    public class AuthorizeDCode : AuthorizeAttribute
    {
            //private ICommonService _commonService;
        
            //public AuthorizeDCode(ICommonService commonService)
            //{
            //    _commonService = commonService;
            //}
            //public override void OnAuthorization(AuthorizationContext filterContext)
            //{
            //    var userIdentity = WindowsIdentity.GetCurrent();

            //    if(SessionHelper.Retrieve(Constants.UserContext) == null)
            //    {
            //        var userContext = _commonService.GetCurrentUserContext();
            //        SessionHelper.Save(Constants.UserContext, userContext);
            //    }
            
            //}
    }
}

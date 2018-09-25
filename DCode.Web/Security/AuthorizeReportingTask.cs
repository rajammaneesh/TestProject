using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DCode.Common;
using Ninject;
using DCode.Web.App_Start;
using System.Reflection;
using DCode.Services.Common;
using System.Security.Principal;

namespace DCode.Web.Security
{
    public class AuthorizeReportingTask : AuthorizeAttribute
    {
        //private ICommonService _commonService;
        private readonly Lazy<IKernel> _kernel = new Lazy<IKernel>(() =>
        {
            IKernel kernel = null;
            try
            {
                kernel = NinjectWebCommon.CreateKernel();
                kernel.Load(Assembly.GetExecutingAssembly());
                return kernel;
            }
            catch
            {
                if (kernel != null) kernel.Dispose();
                throw;
            }
        });

        public override void OnAuthorization(AuthorizationContext filterContext)
        {

            var userIdentity = ConfigurationManager.AppSettings[Constants.UseWindowsIdentity].Equals(Constants.True) ? WindowsIdentity.GetCurrent() : HttpContext.Current.User.Identity;
            if (userIdentity != null && userIdentity.IsAuthenticated)
            {
                var commonService = _kernel.Value.Get<ICommonService>();

                //if (SessionHelper.Retrieve(Constants.UserContext) == null)
                {
                    var userName = userIdentity.Name.Substring(userIdentity.Name.IndexOf(@"\") + 1);

                    var userContext = commonService.GetCurrentUserContext(userName);

                    var isTechXAccessible = commonService.GetTechXAccess();
                    var reportingUsers = ConfigurationManager.AppSettings["ReportingUsers"];
                    var isReportingPageAccessible = reportingUsers.Split(',').Any(x => x == userContext.EmailId);

                    if (!isTechXAccessible || !isReportingPageAccessible)
                    {
                        filterContext.Result = new HttpUnauthorizedResult();

                        return;
                    }

                    SessionHelper.Save(Constants.UserContext, userContext);
                }
            }
            else
            {
                filterContext.Result = new HttpUnauthorizedResult();
                return;
            }

        }


    }
}
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

            var commonService = _kernel.Value.Get<ICommonService>();
            // var userContext = _commonService.GetCurrentUserContext();
            //var userName = userIdentity.Name.Substring(userIdentity.Name.IndexOf(@"\") + 1);
            var userContext = commonService.GetCurrentUserContext();

            var internalUsers = ConfigurationManager.AppSettings["ReportingUsers"];

            if (internalUsers != null
                && internalUsers.Split(',').Any(x => x == userContext.EmailId))
            {

                //return View("~/Views/Reporting/SplitPerOffering.cshtml");

            }
            else
            {
                filterContext.Result = new HttpUnauthorizedResult();
                return;
            }


        }

        
    }
}
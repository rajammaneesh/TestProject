using DCode.Common;
using DCode.Models.Common;
using DCode.Services.Common;
using DCode.Web.App_Start;
using Ninject;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;

namespace DCode.Web.Security
{
    public class AuthorizeDCode : AuthorizeAttribute
    {
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

        public AuthorizeDCode()
        {
        }
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var userIdentity = ConfigurationManager.AppSettings[Constants.UseWindowsIdentity].Equals(Constants.True) ? WindowsIdentity.GetCurrent() : HttpContext.Current.User.Identity;

            if (userIdentity != null && userIdentity.IsAuthenticated)
            {
                var commonService = _kernel.Value.Get<ICommonService>();
                if (SessionHelper.Retrieve(Constants.UserContext) == null)
                {
                    var userName = userIdentity.Name.Substring(userIdentity.Name.IndexOf(@"\") + 1);
                    var userContext = commonService.GetCurrentUserContext(userName);
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
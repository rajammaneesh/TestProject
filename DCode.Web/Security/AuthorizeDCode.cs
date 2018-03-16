using DCode.Common;
using DCode.Services.Common;
using DCode.Web.App_Start;
using Ninject;
using System;
using System.Configuration;
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

            var isTestFlowEnabled = ConfigurationManager.AppSettings[Constants.EnableTestFlow].ToString().Equals(Constants.True);

            var mockUserContextValue = Convert.ToString(ConfigurationManager.AppSettings[Constants.MockUserLogin]);

            string userName = string.Empty;

            if (isTestFlowEnabled && !string.IsNullOrWhiteSpace(mockUserContextValue))
            {
                userName = mockUserContextValue;
            }
            else if (userIdentity != null && userIdentity.IsAuthenticated)
            {
                userName = userIdentity.Name.Substring(userIdentity.Name.IndexOf(@"\") + 1);
            }

            if (!string.IsNullOrWhiteSpace(userName))
            {
                var commonService = _kernel.Value.Get<ICommonService>();

                if (SessionHelper.Retrieve(Constants.UserContext) == null)
                {
                    var userContext = commonService.GetCurrentUserContext(userName);

                    var isTechXAccessible = commonService.GetTechXAccess();

                    if (!isTechXAccessible)
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
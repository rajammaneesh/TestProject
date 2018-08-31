using DCode.Common;
using DCode.Models.User;
using System.Web.Mvc;
using static DCode.Models.Enums.Enums;

namespace DCode.Web.Security
{
    public class AuthorizeRoute : AuthorizeAttribute
    {
        private Role Role { get; set; }
        public AuthorizeRoute(params Role[] role)
        {
            if(role != null)
            {
                this.Role = role[0];
            }
        }
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var userContext = SessionHelper.Retrieve(Constants.UserContext) as UserContext;
            if (userContext.Role != this.Role)
            {
                filterContext.Result = new HttpUnauthorizedResult();
                return;
            }
        }
    }
}
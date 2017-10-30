using DCode.Common;
using DCode.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DCode.Web.Security
{
    public class AuthorizeRoute : AuthorizeAttribute
    {
        private Enums.Role Role { get; set; }
        public AuthorizeRoute(params Enums.Role[] role)
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
using System.Web.Mvc;
using static DCode.Common.Enums;

namespace DCode.Common
{
    public static class MvcHelpers
    {
        public static RedirectResult ErrorRedirectUsingRedirectType(
            this Controller controller,
            ErrorRedirectType redirectType)
        {
            controller.TempData[Constants.ErrorRedirectType] = redirectType;

            return new RedirectResult("/error");
        }
    }
}

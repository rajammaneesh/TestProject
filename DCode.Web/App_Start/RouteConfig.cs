using System.Web.Mvc;
using System.Web.Routing;

namespace DCode.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            //routes.IgnoreRoute("elmah.axd");

            //routes.MapRoute(
            //    name: "AngularJsRouting",
            //    url: "{*.}",
            //    defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            //);

            //   routes.MapRoute(
            //    name: "Default",
            //    url: "{*any}",
            //    defaults: new { controller = "Error", action = "Index", id = UrlParameter.Optional }
            //);

            routes.MapRoute(
                 name: "Default",
                 url: "{controller}/{action}/{id}",
                 defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
             );

            routes.MapRoute(
                name: "ErrorPage",
                url: "error/",
                defaults: new { controller = "Error", action = "Index" }
            );
        }
    }
}

using DCode.Data.ReportingRepository;
using DCode.Data.TaskRepository;
using DCode.Data.UserRepository;
using DCode.Services.Reporting;
using System;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace DCode.Web
{
    public class MvcApplication : HttpApplication
    {
        private readonly IReportingService _reportingService;

        public MvcApplication()
        {
            _reportingService = new ReportingService(
                new TaskRepository(
                    new Data.DbContexts.TaskDbContext()),

                new UserRepository(
                  new Data.DbContexts.UserDbContext()),

                new DailyUsageStatisticsRepository(
                    new Data.DbContexts.ReportingDbContext()));
        }
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            _reportingService.UpdateDailySiteVisitCount();
        }
    }

}

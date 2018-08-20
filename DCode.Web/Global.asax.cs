using DCode.Data.DbContexts;
using DCode.Data.LogRepository;
using DCode.Data.MetadataRepository;
using DCode.Data.ReportingRepository;
using DCode.Data.RequestorRepository;
using DCode.Data.TaskRepository;
using DCode.Data.UserRepository;
using DCode.Services.Common;
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
            var commonService = new CommonService(new TaskRepository(
                    new TaskDbContext()),
                new DCode.Models.User.UserContext(),
                new LogRepository(
                    new LogDbContext()),
                new Services.ModelFactory.LogModelFactory(),
                new RequestorRepository(new TaskDbContext()),
                new UserRepository(new UserDbContext()),
                new Services.ModelFactory.UserModelFactory(),
                new Services.ModelFactory.ApplicantSkillModelFactory(),
                new Services.ModelFactory.SkillModelFactory(),
                new Services.ModelFactory.SuggestionModelFactory(),
                new ServiceLineRepository(new MetadataDbContext()),
                new Services.ModelFactory.ServiceLineModelFactory(),
                new TaskTypeRepository(new MetadataDbContext()),
                new Services.ModelFactory.TaskTypeModelFactory(),
                new Services.ModelFactory.OfferingModelFactory(),
                new Services.ModelFactory.UserPointsModelFactory(),
                new Services.ModelFactory.ApprovedApplicantModelFactory(),
                new Services.ModelFactory.PortfolioModelFactory(),
                new OfferingRepository(new MetadataDbContext()),
                new ApprovedApplicantRepository(new MetadataDbContext()),
                new PortfolioRepository(new MetadataDbContext()),
                new UserPointsRepository(new MetadataDbContext())
                );


            _reportingService = new ReportingService(
                 new TaskRepository(
                    new TaskDbContext()),

                     new UserRepository(
                     new UserDbContext()),

                     new DailyUsageStatisticsRepository(new ReportingDbContext()),

                     new DbQuueryManager(),

                     commonService,

                     new Services.ModelFactory.TaskModelFactory(commonService));
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

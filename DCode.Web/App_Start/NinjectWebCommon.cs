[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(DCode.Web.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(DCode.Web.App_Start.NinjectWebCommon), "Stop")]

namespace DCode.Web.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using DCode.Services.Requestor;
    using DCode.Data.TaskRepository;
    using DCode.Services.Common;
    using DCode.Data.RequestorRepository;
    using Services.Task;
    using DCode.Data.LogRepository;
    using DCode.Services.Contributor;
    using DCode.Data.ContributorRepository;
    using Data.MetadataRepository;
    using Services.Reporting;
    using Data.ReportingRepository;
    using Data.UserRepository;
    using DCode.Models.Management;
    using Data.DbContexts;
    using Ninject.Web.Common;
    using Ninject;
    using Ninject.Web.Common.WebHost;
    using DCode.Services.Email;
    using DCode.Data.EmailRepository;

    public static class NinjectWebCommon
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }

        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }

        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        public static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            //Service instantiation
            kernel.Bind<IRequestorService>().To<RequestorService>();
            kernel.Bind<ICommonService>().To<CommonService>();
            kernel.Bind<ITask>().To<Task>();
            kernel.Bind<IContributorService>().To<ContributorService>();
            kernel.Bind<IReportingService>().To<ReportingService>();

            //Repo instantiation
            kernel.Bind<ITaskRepository>().To<TaskRepository>();
            kernel.Bind<IRequestorRepository>().To<RequestorRepository>();
            kernel.Bind<ILogRepository>().To<LogRepository>();
            kernel.Bind<IUserRepository>().To<UserRepository>();
            kernel.Bind<IContributorRepository>().To<ContributorRepository>();
            kernel.Bind<IServiceLineRepository>().To<ServiceLineRepository>();
            kernel.Bind<IOfferingRepository>().To<OfferingRepository>();
            kernel.Bind<IApprovedApplicantRepository>().To<ApprovedApplicantRepository>();
            kernel.Bind<IPortfolioRepository>().To<PortfolioRepository>();
            kernel.Bind<ITaskTypeRepository>().To<TaskTypeRepository>();
            kernel.Bind<IDailyUsageStatisticsRepository>().To<DailyUsageStatisticsRepository>();
            kernel.Bind<IDataManagement>().To<DbQuueryManager>();
            kernel.Bind<IUserPointsRepository>().To<UserPointsRepository>();
            kernel.Bind<IEmailTrackerService>().To<EmailTrackerService>();
            kernel.Bind<IEmailRepository>().To<EmailRepository>();
        }

    }
}

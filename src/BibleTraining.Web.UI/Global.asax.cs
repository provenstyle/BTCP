namespace BibleTraining.Web.UI
{
    using System.Reflection;
    using System.Web;
    using System.Web.Http.Cors;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;
    using Castle.Facilities.Logging;
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.Resolvers.SpecializedResolvers;
    using Castle.Windsor;
    using Castle.Windsor.Installer;
    using Improving.AspNet;
    using Improving.Highway.Data.Scope.Repository;
    using Improving.MediatR;
    using NLog;

    public class BibleTrainingApplication : HttpApplication
    {
        private IWindsorContainer _container;
        private Castle.Core.Logging.ILogger _logger;

        protected void Application_Start()
        {
            GlobalDiagnosticsContext.Set("ApplicationName", Assembly.GetExecutingAssembly().GetName().Name);

            ConfigureContainer();

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

            _logger = _container.Resolve<Castle.Core.Logging.ILogger>();
            _logger.InfoFormat("Started {0}", nameof(BibleTrainingApplication));
        }

        protected void Application_End()
        {
            _logger.InfoFormat("Ending {0}", nameof(BibleTrainingApplication));
        }

        private void ConfigureContainer()
        {
            _container = new WindsorContainer();
            _container.Kernel.Resolver.AddSubResolver(
                new CollectionResolver(_container.Kernel, true));
            _container.AddFacility<LoggingFacility>(f => f.UseNLog());
            _container.Install(
                new MediatRInstaller(
                    Classes.FromThisAssembly(),
                    Classes.FromAssemblyContaining<IBibleTrainingDomain>()
                ),
                new RepositoryInstaller(
                    Classes.FromAssemblyContaining<IBibleTrainingDomain>()
                ),
                new WebApiInstaller(Classes.FromThisAssembly())
                    .EnableCors(new EnableCorsAttribute("*", "*", "*"))
                    .UseCamelCaseJsonPropertyNames()
                    .UseDefaultRoutesAndAttributeRoutes()
                    .UseJsonAsTheDefault()
                    .TypeNameHandling()
                    .IgnoreNulls()
                    .UseGlobalExceptionLogging()
                    .UseFilters(filters => filters.Add(new ServiceBusExceptionFilter())
                ),
                new MvcInstaller(Classes.FromThisAssembly())
                    .UseFeaturePaths()
                    .UseDefaultRoutes()
                    .UseFluentValidation()
                    .UseFilters(filters => filters.Add(new HandleErrorAttribute())
                ),
                new TypedConfigurationInstaller(Types.FromThisAssembly()),
                FromAssembly.Containing<IBibleTrainingDomain>()
             );
        }
    }
}

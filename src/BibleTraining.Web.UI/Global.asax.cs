namespace BibleTraining.Web.UI
{
    using System.Configuration;
    using System.Linq.Dynamic;
    using System.Reflection;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Http;
    using System.Web.Http.Cors;
    using System.Web.Mvc;
    using Castle.Facilities.Logging;
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.Resolvers.SpecializedResolvers;
    using Castle.Windsor;
    using Castle.Windsor.Installer;
    using DataTables.AspNet.WebApi2;
    using Entities;
    using Improving.AspNet;
    using Improving.Highway.Data.Scope.Repository;
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

            GlobalConfiguration.Configure(x => x.RegisterDataTables());

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

            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new TranspiledFeatureViewLocationRazorViewEngine());

            //Take the first EF hit
            Task.Factory.StartNew(() =>
              {
                  var repository = _container.Resolve<IRepository<IBibleTrainingDomain>>();
                  using (repository.Scopes.CreateReadOnly())
                  {
                      repository.DomainContext.AsQueryable<Course>().Count();
                  }
              });
        }
    }

    public class TranspiledFeatureViewLocationRazorViewEngine : RazorViewEngine
    {
        public TranspiledFeatureViewLocationRazorViewEngine()
        {
            var folder = ConfigurationManager.AppSettings["ViewFolder"];

            if (string.IsNullOrEmpty(folder))
                throw new ConfigurationErrorsException(
                    "You must configure a 'ViewFolder' in the Web.config");

            var featureFolderViewLocationFormats = new[]
            {
                // First: Look in the feature folder
                $"~/{folder}/Features/{{1}}/{{0}}/View.cshtml",
                $"~/{folder}/Features/{{1}}/Shared/{{0}}.cshtml",
                $"~/{folder}/Features/Shared/{{0}}.cshtml",
                // If needed: standard  locations
                "~/Views/{1}/{0}.cshtml",
                "~/Views/Shared/{0}.cshtml"
            };

            ViewLocationFormats        = featureFolderViewLocationFormats;
            MasterLocationFormats      = featureFolderViewLocationFormats;
            PartialViewLocationFormats = featureFolderViewLocationFormats;

            var defaultAreaViewLocations = new[]
            {
                "~/Areas/{2}/Views/{1}/{0}.cshtml",
                "~/Areas/{2}/Views/{1}/{0}.vbhtml",
                "~/Areas/{2}/Views/Shared/{0}.cshtml",
                "~/Areas/{2}/Views/Shared/{0}.vbhtml"
            };

            AreaMasterLocationFormats      = defaultAreaViewLocations;
            AreaPartialViewLocationFormats = defaultAreaViewLocations;
            AreaViewLocationFormats        = defaultAreaViewLocations;

        }
    }
}

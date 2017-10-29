namespace BibleTraining.Web.UI
{
    using System.Configuration;
    using System.Linq.Dynamic;
    using System.Reflection;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Http;
    using System.Web.Http.ExceptionHandling;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Castle.Facilities.Logging;
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.Resolvers.SpecializedResolvers;
    using Castle.Windsor;
    using Castle.Windsor.Installer;
    using DataTables.AspNet.WebApi2;
    using Entities;
    using Improving.Highway.Data.Scope.Repository;
    using Miruken.AspNet.Castle;
    using Miruken.Castle;
    using NLog;
    using Miruken.Context;
    using Miruken.Mediate.Castle;
    using Miruken.Validate.Castle;

    public class BibleTrainingApplication : HttpApplication
    {
        private IWindsorContainer _container;
        private Castle.Core.Logging.ILogger _logger;

        protected void Application_Start()
        {
            GlobalDiagnosticsContext.Set("ApplicationName", Assembly.GetExecutingAssembly().GetName().Name);

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            GlobalConfiguration.Configure(x => x.RegisterDataTables());

            ConfigureContainer();

            _logger = _container.Resolve<Castle.Core.Logging.ILogger>();
            _logger.InfoFormat("Started {0}", nameof(BibleTrainingApplication));
            GlobalConfiguration.Configuration.Services.Add(typeof(IExceptionLogger), new UnhandledExceptionLogger(_logger));

        }

        protected void Application_End()
        {
            _logger.InfoFormat("Ending {0}", nameof(BibleTrainingApplication));
        }

        private void ConfigureContainer()
        {
            var appContext = new Context();
            _container = new WindsorContainer();
            _container.AddFacility<LoggingFacility>(f => f.UseNLog());
            _container.Install(
                new FeaturesInstaller(
                    new ConfigurationFeature(),
                    new HandleFeature(),
                    new ValidateFeature(),
                    new MediateFeature()
                        .WithStandardMiddleware(),
                    new AspNetFeature(appContext)
                        .WithMvc(this)
                        .WithWebApi(GlobalConfiguration.Configuration)
                    ).Use(
                        Types.FromThisAssembly(),
                        Classes.FromAssemblyContaining<IBibleTrainingDomain>()
                    ),
                new RepositoryInstaller(
                    Classes.FromAssemblyContaining<IBibleTrainingDomain>()
                ),
                FromAssembly.Containing<IBibleTrainingDomain>()
            );

            _container.Kernel.Resolver.AddSubResolver(
                new CollectionResolver(_container.Kernel, true));
            _container.Kernel.AddHandlersFilter(new ContravariantFilter());
            appContext.AddHandlers(new WindsorHandler(_container));

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

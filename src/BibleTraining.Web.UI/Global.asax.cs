using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace BibleTraining.Web.UI
{
    using System.Configuration;
    using System.Reflection;
    using System.Web.Http.Cors;
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
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

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
                FromAssembly.This(),
                FromAssembly.Containing<IBibleTrainingDomain>(),
                new WebApiInstaller(Classes.FromThisAssembly())
                    .EnableCors(new EnableCorsAttribute("*", "*", "*"))
                    .UseCamelCaseJsonPropertyNames()
                    .UseDefaultRoutesAndAttributeRoutes()
                    .UseJsonAsTheDefault()
                    .TypeNameHandling()
                    .IgnoreNulls()
                    .UseGlobalExceptionLogging(),
                new MediatRInstaller(
                    Classes.FromThisAssembly(),
                    Classes.FromAssemblyContaining<IBibleTrainingDomain>()
                ),
                new RepositoryInstaller(
                    Classes.FromAssemblyContaining<IBibleTrainingDomain>()
                ),
                FromAssembly.Containing<IBibleTrainingDomain>()
             );
        }
    }
}

namespace IntegrationTests.Scenarios
{
    using System;
    using System.Linq;
    using System.Data.Entity.Validation;
    using System.Transactions;
    using BibleTraining;
    using BibleTraining.Api;
    using BibleTraining.Entities;
    using Castle.MicroKernel.Registration;
    using Castle.Windsor;
    using Castle.Windsor.Installer;
    using Highway.Data;
    using Improving.Highway.Data.Scope.Repository;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Miruken.Callback;
    using Miruken.Castle;
    using Miruken.Mediate.Castle;
    using Miruken.Validate.Castle;
    using Ploeh.AutoFixture;
    using TestInfrastructure;

    public abstract class BibleTrainingScenario : TransactionalScenario
    {
        protected IRepository<IBibleTrainingDomain> Repository;
        protected Fixture Fixture;
        protected DataContext Context => (DataContext) Repository.Context;
        protected IHandler Handler;

        [TestInitialize]
        public virtual void TestInitialize()
        {
            var container = new WindsorContainer();
            container.Install(
                new FeaturesInstaller(
                    new ConfigurationFeature(),
                    new HandleFeature(),
                    new ValidateFeature(),
                    new MediateFeature()
                        .WithStandardMiddleware()
                    ).Use(
                        Types.FromThisAssembly(),
                        Classes.FromAssemblyContaining<IBibleTrainingDomain>()
                    ),
                new RepositoryInstaller(
                    Classes.FromAssemblyContaining<IBibleTrainingDomain>()
                ),
                FromAssembly.Containing<IBibleTrainingDomain>()
            );

            Repository = container.Resolve<IRepository<IBibleTrainingDomain>>();
            Handler    = new WindsorHandler(container).Resolve();
            Fixture    = new BibleTrainingFixture().Fixture;

        }


        protected void AssertCanSelectTopOne<T>() where T : class
        {
            using (Repository.Scopes.Create())
            {
                Context.AsQueryable<T>().FirstOrDefault();
            }
        }

        protected void AssertCanCreateEntity<T>(Action<T> configure = null) where T : class
        {
            using (new TransactionScope(TransactionScopeOption.RequiresNew))
            {
                try
                {
                    using (Repository.Scopes.Create())
                    {
                        var entity = Fixture.Create<T>();
                        configure?.Invoke(entity);
                        Context.Add(entity);
                        Context.Commit();
                    }
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (var eve in ex.EntityValidationErrors)
                    foreach (var ve in eve.ValidationErrors)
                        Console.WriteLine($"${eve.Entry.Entity}\r\n  ${ve.PropertyName}\r\n    ${ve.ErrorMessage}\r\n");

                    throw;
                }
            }
        }
    }
}

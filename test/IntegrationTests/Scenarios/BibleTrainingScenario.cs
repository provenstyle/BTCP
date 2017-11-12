namespace IntegrationTests.Scenarios
{
    using System;
    using System.Linq;
    using System.Data.Entity.Validation;
    using System.Transactions;
    using BibleTraining;
    using BibleTraining.Entities;
    using Castle.MicroKernel.Registration;
    using Castle.Windsor;
    using Castle.Windsor.Installer;
    using Highway.Data;
    using Improving.Highway.Data.Scope.Repository;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Ploeh.AutoFixture;

    public abstract class BibleTrainingScenario
    {
        protected IRepository<IBibleTrainingDomain> Repository;
        protected Fixture Fixture;
        protected DataContext Context => (DataContext) Repository.Context;

        [TestInitialize]
        public void TestInitialize()
        {
            var container = new WindsorContainer();
            container.Install(
                new RepositoryInstaller(
                    Classes.FromAssemblyContaining<IBibleTrainingDomain>()
                ),
                FromAssembly.Containing<IBibleTrainingDomain>()
            );

            Repository = container.Resolve<IRepository<IBibleTrainingDomain>>();

            Fixture = new Fixture();

            Fixture.Customize<Entity>(c => c.Without(x => x.RowVersion));

            Fixture.Customize<Person>(c =>
                c.Without(x => x.Addresses)
                 .Without(x => x.Emails)
                 .Without(x => x.Phones));
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

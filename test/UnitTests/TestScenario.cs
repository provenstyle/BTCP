namespace UnitTests
{
    using System.Linq;
    using System.Threading.Tasks;
    using BibleTraining;
    using BibleTraining.Api;
    using BibleTraining.Entities;
    using Castle.DynamicProxy;
    using Castle.MicroKernel.Lifestyle;
    using Castle.MicroKernel.Registration;
    using Castle.Windsor;
    using FizzWare.NBuilder;
    using FluentValidation;
    using Highway.Data;
    using Improving.Highway.Data.Scope.Repository;
    using Infrastructure;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Miruken.Callback;
    using Miruken.Castle;
    using Miruken.Mediate.Castle;
    using Miruken.Validate.Castle;
    using Rhino.Mocks;

    public class TestScenario
    {
        protected RandomGenerator _random;
        protected IWindsorContainer _container;
        protected IDomainContext<IDomain> _context;
        protected IHandler _handler;

        [TestInitialize]
        public virtual void TestInitialize()
        {
            _random = new RandomGenerator();
            _context = MockRepository.GenerateMock<IDomainContext<IDomain>>();

            _container = new WindsorContainer();

            BeforeContainer(_container);

            _container.Register(
                    Component.For<IDomainContext<IBibleTrainingDomain>>().Instance(_context))
                .Install(
                    new FeaturesInstaller(
                        new HandleFeature(),
                        new ValidateFeature(),
                        new MediateFeature().WithStandardMiddleware())
                        .Use(Classes.FromThisAssembly(),
                             Classes.FromAssemblyContaining<IBibleTrainingDomain>()),
                    new RepositoryInstaller(
                        Classes.FromAssemblyContaining<IBibleTrainingDomain>())
                );

            _handler = new WindsorHandler(_container).Resolve();

            AfterContainer(_container);
        }

        protected virtual void BeforeContainer(IWindsorContainer container)
        {
        }

        protected virtual void AfterContainer(IWindsorContainer container)
        {
        }

        [TestCleanup]
        public virtual void TestCleanup()
        {
            _container?.Dispose();
        }

        protected virtual void SetupChoices()
        {
            GenerateEntity<BibleTraining.Entities.Course>();
            GenerateEntity<BibleTraining.Entities.Person>();
            GenerateEntity<BibleTraining.Entities.Email>();
            GenerateEntity<BibleTraining.Entities.EmailType>();
            GenerateEntity<BibleTraining.Entities.Address>();
            GenerateEntity<BibleTraining.Entities.AddressType>();
            GenerateEntity<BibleTraining.Entities.Phone>();
            GenerateEntity<BibleTraining.Entities.PhoneType>();
        }

        private void GenerateEntity<T>() where T : class
        {
            _context.Stub(p => p.AsQueryable<T>())
                .Return(TestChoice<T>(3).TestAsync());
        }

        protected static IQueryable<T> TestChoice<T>(int howMany)
        {
            return Builder<T>.CreateListOfSize(howMany).Build().AsQueryable();
        }

        public TestScenario ExpectContextToSaveAsync()
        {
            _context
                .Stub(x => x.CommitAsync())
                .Return(Task.FromResult(0));
            return this;
        }

        protected void AssertValidationErrors<T, R>(R request, params string[] errors) where T : IValidator<R>
        {
            var validator = GetValidator<T, R>();
            var results = validator.Validate(request);
            var actual = results.Errors.Select(e => e.ErrorMessage).ToArray();
            foreach (var error in errors)
                CollectionAssert.Contains(actual, error);
        }

        protected void AssertNoValidationErrors<T, R>(R request) where T : IValidator<R>
        {
            var validator = GetValidator<T, R>();
            var results = validator.Validate(request);
            if (results.Errors.Count > 0)
                Assert.Fail($"Expected no validation errors, but found {results.Errors.Count}");
        }

        protected IValidator GetValidator<T, R>() where T : IValidator<R>
        {
            using (_container.BeginScope())
            {
                return _container.ResolveAll<IValidator<R>>()
                    .First(v => ProxyUtil.GetUnproxiedType(v).IsAssignableFrom(typeof(T)));
            }
        }

        protected void AssertResourcesMapToEntities(Entity entity, Resource<int?> resource)
        {
            Assert.AreEqual(resource.Id,         entity.Id);
            Assert.AreEqual(resource.RowVersion, entity.RowVersion);
            Assert.AreEqual(resource.ModifiedBy, entity.ModifiedBy);

            Assert.IsTrue(resource.Modified < entity.Modified);
        }

        protected void AssertEntitiesMapToResources(Resource<int?> resource, Entity entity)
        {
            Assert.AreEqual(entity.Id,         resource.Id);
            Assert.AreEqual(entity.RowVersion, resource.RowVersion);
            Assert.AreEqual(entity.Created,    resource.Created);
            Assert.AreEqual(entity.CreatedBy,  resource.CreatedBy);
            Assert.AreEqual(entity.Modified,   resource.Modified);
            Assert.AreEqual(entity.ModifiedBy, resource.ModifiedBy);
        }
    }
}
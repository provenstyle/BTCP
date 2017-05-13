namespace BibleTraining.Test
{
    using System.Linq;
    using System.Threading.Tasks;
    using Api.Address;
    using Api.AddressType;
    using Api.EmailType;
    using Api.Course;
    using Api.Email;
    using Api.Person;
    using Api.Phone;
    using BibleTraining;
    using Castle.DynamicProxy;
    using Castle.MicroKernel.Lifestyle;
    using Castle.MicroKernel.Registration;
    using Castle.Windsor;
    using Castle.Windsor.Installer;
    using Entities;
    using FizzWare.NBuilder;
    using FluentValidation;
    using Highway.Data;
    using Improving.Highway.Data.Scope.Repository;
    using Improving.MediatR;
    using Improving.MediatR.Cache;
    using Infrastructure;
    using MediatR;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Phone;
    using PhoneType;
    using Rhino.Mocks;

    public class TestScenario
    {
        protected RandomGenerator _random;
        protected IWindsorContainer _container;
        protected IDomainContext<IDomain> _context;
        protected IMediator _mediator;

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
                    new MediatRInstaller(
                        Classes.FromThisAssembly(),
                        Classes.FromAssemblyContaining<IBibleTrainingDomain>()),
                    new RepositoryInstaller(
                        Classes.FromAssemblyContaining<IBibleTrainingDomain>()),
                    FromAssembly.Containing<IBibleTrainingDomain>()
                );

            AfterContainer(_container);

            _mediator = _container.Resolve<IMediator>();
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
            InvalidateCache(new GetCourses());
            InvalidateCache(new GetPeople());
            InvalidateCache(new GetEmails());
            InvalidateCache(new GetEmailTypes());
            InvalidateCache(new GetAddresses());
            InvalidateCache(new GetAddressTypes());
            InvalidateCache(new GetPhones());
            InvalidateCache(new GetPhoneTypes());

            _context.Stub(p => p.AsQueryable<Entities.Course>())
                .Return(TestChoice<Entities.Course>(3).TestAsync());

            _context.Stub(p => p.AsQueryable<Entities.Person>())
                .Return(TestChoice<Entities.Person>(3).TestAsync());

            _context.Stub(p => p.AsQueryable<Entities.Email>())
                .Return(TestChoice<Entities.Email>(3).TestAsync());

            _context.Stub(p => p.AsQueryable<Entities.EmailType>())
                .Return(TestChoice<Entities.EmailType>(3).TestAsync());

            _context.Stub(p => p.AsQueryable<Entities.Address>())
                .Return(TestChoice<Entities.Address>(3).TestAsync());

            _context.Stub(p => p.AsQueryable<Api.AddressType.AddressType>())
                .Return(TestChoice<Api.AddressType.AddressType>(3).TestAsync());

            _context.Stub(p => p.AsQueryable<Entities.Phone>())
                .Return(TestChoice<Entities.Phone>(3).TestAsync());

            _context.Stub(p => p.AsQueryable<Entities.PhoneType>())
                .Return(TestChoice<Entities.PhoneType>(3).TestAsync());
        }

        protected void InvalidateCache<TResponse>(Request.WithResponse<TResponse> request)
            where TResponse : class
        {
            _mediator.SendAsync(request.InvalidateCache());
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
            var mediatR = _container.GetChildContainer("Improving.MediatR");
            using (mediatR.BeginScope())
            {
                return mediatR.ResolveAll<IValidator<R>>()
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

    class TestScenarioImpl : TestScenario
    {
    }
}
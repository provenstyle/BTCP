namespace BibleTraining.Test.PhoneType
{
}

namespace BibleTraining.Test.PhoneType
{
    using Entities;
    using EntityTypeConfigurations;

    public class PhoneTypeEntityTypeConfiguration : BaseEntityTypeConfiguration<PhoneType>
    {
        public PhoneTypeEntityTypeConfiguration ()
        {
            ToTable(nameof(PhoneType));
        }
    }
}

namespace BibleTraining.Test.PhoneType
{
}

namespace BibleTraining.Test.PhoneType
{
    using Api.PhoneType;

    public class PhoneTypeResult
    {
        public PhoneTypeData[] PhoneTypes { get; set; }
    }
}

namespace BibleTraining.Test.PhoneType
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Api.PhoneType;
    using Entities;
    using Improving.Highway.Data.Scope.Repository;
    using Improving.MediatR;
    using Improving.MediatR.Pipeline;
    using MediatR;

    [RelativeOrder(Stage.Validation - 1)]
    public class PhoneTypeAggregateHandler :
        IAsyncRequestHandler<CreatePhoneType, PhoneTypeData>,
        IAsyncRequestHandler<GetPhoneTypes, PhoneTypeResult>,
        IAsyncRequestHandler<UpdatePhoneType, PhoneTypeData>,
        IRequestMiddleware<UpdatePhoneType, PhoneTypeData>,
        IAsyncRequestHandler<RemovePhoneType, PhoneTypeData>,
        IRequestMiddleware<RemovePhoneType, PhoneTypeData>
    {
        private readonly IRepository<IBibleTrainingDomain> _repository;

        public PhoneType PhoneType { get; set; }

        public PhoneTypeAggregateHandler(IRepository<IBibleTrainingDomain> repository)
        {
            _repository = repository;
        }

        #region Create PhoneType

        public async Task<PhoneTypeData> Handle(CreatePhoneType message)
        {
            using(var scope = _repository.Scopes.Create())
            {
                var phoneType = new PhoneType().Map(message.Resource);
                phoneType.Created = DateTime.Now;

                _repository.Context.Add(phoneType);

                var data = new PhoneTypeData();

                await scope.SaveChangesAsync((dbScope, count) =>
                                                 {
                                                     data.Id = phoneType.Id;
                                                     data.RowVersion = phoneType.RowVersion;
                                                 });

                return data;
            }
        }

        #endregion

        #region Get PhoneType

        public async Task<PhoneTypeResult> Handle(GetPhoneTypes message)
        {
            using(_repository.Scopes.CreateReadOnly())
            {
                var phoneTypes = (await _repository.FindAsync(new GetPhoneTypesById(message.Ids){
                    KeyProperties = message.KeyProperties
                })).Select(x => new PhoneTypeData().Map(x)).ToArray();

                return new PhoneTypeResult
                {
                    PhoneTypes = phoneTypes
                };
            }
        }

        #endregion

        #region Update PhoneType

        public async Task<PhoneTypeData> Apply(UpdatePhoneType request, Func<UpdatePhoneType, Task<PhoneTypeData>> next)
        {
            using (var scope = _repository.Scopes.Create())
            {
                var resource = request.Resource;
                if (PhoneType == null && resource != null)
                {
                    PhoneType = (await _repository
                             .FindAsync(new GetPhoneTypesById(resource.Id)))
                        .FirstOrDefault();
                    Env.Use(PhoneType);
                }

                var result = await next(request);
                await scope.SaveChangesAsync();

                result.RowVersion = PhoneType?.RowVersion;
                return result;
            }
        }

        public Task<PhoneTypeData> Handle(UpdatePhoneType request)
        {
            PhoneType.Map(request.Resource);

            return Task.FromResult(new PhoneTypeData
            {
                Id = request.Resource.Id
            });
        }

        #endregion

        #region Remove PhoneType

        public async Task<PhoneTypeData> Apply(
            RemovePhoneType request, Func<RemovePhoneType, Task<PhoneTypeData>> next)
        {
            using (var scope = _repository.Scopes.Create())
            {
                var resource = request.Resource;
                if (PhoneType == null && resource != null)
                {
                    PhoneType = (await _repository
                             .FindAsync(new GetPhoneTypesById(resource.Id)))
                        .FirstOrDefault();
                    Env.Use(PhoneType);
                }

                var result = await next(request);
                await scope.SaveChangesAsync();
                return result;
            }
        }

        public Task<PhoneTypeData> Handle(RemovePhoneType request)
        {
            _repository.Context.Remove(PhoneType);

            return Task.FromResult(new PhoneTypeData
            {
                Id         = PhoneType.Id,
                RowVersion = PhoneType.RowVersion
            });
        }

        #endregion

    }
}

namespace BibleTraining.Test.PhoneType
{
    using Api.PhoneType;
    using Entities;
    using FizzWare.NBuilder;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class PhoneTypeMappingTests : TestScenario
    {
        [TestMethod]
        public void ShouldMapResourcesToEntities()
        {
            var resource = Builder<PhoneTypeData>.CreateNew()
                .With(c => c.RowVersion = new byte[] { 0x01 })
                .Build();
            var entity = new PhoneType().Map(resource);

            AssertResourcesMapToEntities(entity, resource);

            Assert.AreEqual(resource.Name, entity.Name);
            Assert.AreEqual(resource.Description, entity.Description);
        }

        [TestMethod]
        public void ShouldMapDefaultResourcesToEntitiesWithNoErrors()
        {
            new PhoneType().Map(new PhoneTypeData());
        }

        [TestMethod]
        public void ShouldMapEntitiesToResources()
        {
            var entity = Builder<PhoneType>.CreateNew()
                .With(c => c.RowVersion = new byte[] { 0x01 })
                .Build();
            var resource = new PhoneTypeData().Map(entity);

            AssertEntitiesMapToResources(resource, entity);

            Assert.AreEqual(entity.Name,        resource.Name);
            Assert.AreEqual(entity.Description, resource.Description);
        }

        [TestMethod]
        public void ShouldMapDefaultEntitiesToResourcesWithNoErrors()
        {
            new PhoneTypeData().Map(new PhoneType());
        }
    }
}

namespace BibleTraining.Test.PhoneType
{
    using Api.PhoneType;
    using Improving.MediatR;

    public class CreatePhoneType : ResourceAction<PhoneTypeData, int?>
    {
        public CreatePhoneType()
        {
        }

        public CreatePhoneType(PhoneTypeData phoneType)
            : base (phoneType)
        {
        }
    }
}

namespace BibleTraining.Test.PhoneType
{
    using System.Threading.Tasks;
    using Api.PhoneType;
    using Entities;
    using FizzWare.NBuilder;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Rhino.Mocks;

    [TestClass]
    public class CreatePhoneTypeTests : TestScenario
    {
        [TestMethod]
        public async Task ShouldCreatePhoneType()
        {
            var phoneType = Builder<PhoneTypeData>.CreateNew()
                .With(pg => pg.Id = 0).And(pg => pg.RowVersion = null)
                .Build();

            _context.Expect(pg => pg.Add(Arg<PhoneType>.Is.Anything))
                .WhenCalled(inv =>
                                {
                                    var entity = (PhoneType)inv.Arguments[0];
                                    entity.Id         = 1;
                                    entity.RowVersion = new byte[] { 0x01 };
                                    Assert.AreEqual(phoneType.Name, entity.Name);
                                    inv.ReturnValue = entity;
                                }).Return(null);

            _context.Expect(pg => pg.CommitAsync())
                .Return(Task.FromResult(1));

            var result = await _mediator.SendAsync(new CreatePhoneType(phoneType));
            Assert.AreEqual(1, result.Id);
            CollectionAssert.AreEqual(new byte[] { 0x01 }, result.RowVersion);

            _context.VerifyAllExpectations();
        }
    }
}

namespace BibleTraining.Test.PhoneType
{
    using Api.PhoneType;
    using FluentValidation;

    public class CreatePhoneTypeIntegrity : AbstractValidator<CreatePhoneType>
    {
        public CreatePhoneTypeIntegrity()
        {
            RuleFor(x => x.Resource)
                .NotNull()
                .SetValidator(new PhoneTypeDataIntegrity());
        }

        private class PhoneTypeDataIntegrity : AbstractValidator<PhoneTypeData>
        {
            public PhoneTypeDataIntegrity()
            {
                RuleFor(x => x.Name)
                    .NotEmpty();
            }
        }
    }
}

namespace BibleTraining.Test.PhoneType
{
    using Api.PhoneType;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class CreatePhoneTypeIntegrityTests
    {
        private CreatePhoneType createPhoneType;
        private CreatePhoneTypeIntegrity validator;

        [TestInitialize]
        public void TestInitialize()
        {
            createPhoneType =  new CreatePhoneType
            {
                Resource = new PhoneTypeData
                {
                    Name = "a"
                }
            };

            validator = new CreatePhoneTypeIntegrity();
        }

        [TestMethod]
        public void IsValid()
        {
            var result = validator.Validate(createPhoneType);
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void MustHaveName()
        {
            createPhoneType.Resource.Name = string.Empty;
            var result = validator.Validate(createPhoneType);
            Assert.IsFalse(result.IsValid);
        }
    }
}

namespace BibleTraining.Test.PhoneType
{
    using Improving.MediatR;

    public class GetPhoneTypes : Request.WithResponse<PhoneTypeResult>
    {
        public GetPhoneTypes()
        {
            Ids = new int[0];
        }

        public GetPhoneTypes(params int[] ids)
        {
            Ids = ids;
        }

        public int[] Ids { get; set;}

        public bool KeyProperties { get; set; }
    }
}

namespace BibleTraining.Test.PhoneType
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Linq;
    using System.Threading.Tasks;
    using Entities;
    using Infrastructure;
    using Rhino.Mocks;

    [TestClass]
    public class GetPhoneTypesTests : TestScenario
    {
        [TestMethod]
        public async Task ShouldGetPhoneTypes()
        {
            SetupChoices();

            var result = await _mediator.SendAsync(new GetPhoneTypes());
            Assert.AreEqual(3, result.PhoneTypes.Length);

            _context.VerifyAllExpectations();
        }

        [TestMethod]
        public async Task ShouldGetOnlyKeyProperties()
        {
            _context.Stub(p => p.AsQueryable<PhoneType>())
                .Return(TestChoice<PhoneType>(3).TestAsync());

            var result = await _mediator.SendAsync(new GetPhoneTypes { KeyProperties = true });

            Assert.IsTrue(result.PhoneTypes.All(x => x.Name != null));
            Assert.IsTrue(result.PhoneTypes.All(x => x.CreatedBy == null));

            _context.VerifyAllExpectations();
        }
    }
}

namespace BibleTraining.Test.PhoneType
{
    using System.Linq;
    using Entities;
    using Highway.Data;

    public class GetPhoneTypesById : Query<PhoneType>
    {
        public bool KeyProperties { get; set; }

        public GetPhoneTypesById(int? id)
            :this(new []{ id ?? 0 })
        {
        }


        public GetPhoneTypesById(int[] ids)
        {
            ContextQuery = c =>
                               {
                                   var query = Context.AsQueryable<PhoneType>();

                                   if (ids?.Length == 1)
                                   {
                                       var id = ids[0];
                                       query = query.Where(x => x.Id == id);
                                   }
                                   else if (ids?.Length > 1)
                                   {
                                       query = query.Where(x => ids.Contains(x.Id));
                                   }

                                   if (KeyProperties)
                                   {
                                       return query.Select(x => new PhoneType{Id = x.Id, Name = x.Name});
                                   }

                                   return query;
                               };
        }
    }
}

namespace BibleTraining.Test.PhoneType
{
    using Api.PhoneType;
    using Improving.MediatR;

    public class UpdatePhoneType : UpdateResource<PhoneTypeData, int?>
    {
        public UpdatePhoneType()
        {
        }

        public UpdatePhoneType(PhoneTypeData phoneType)
            : base(phoneType)
        {
        }
    }
}

namespace BibleTraining.Test.PhoneType
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Linq;
    using System.Threading.Tasks;
    using Api.PhoneType;
    using Entities;
    using FizzWare.NBuilder;
    using Infrastructure;
    using Rhino.Mocks;

    [TestClass]
    public class UpdatePhoneTypeTests : TestScenario
    {
        [TestMethod]
        public async Task ShouldUpdatePhoneType()
        {
            var phoneType= new PhoneType()
            {
                Id         = 1,
                Name       = "a",
                RowVersion = new byte[] { 0x01 }
            };

            var phoneTypeData = Builder<PhoneTypeData>.CreateNew()
                .With(c => c.Id = 1).And(c => c.RowVersion = new byte[] { 0x01 })
                .Build();

            _context.Expect(c => c.AsQueryable<PhoneType>())
                .Return(new[] { phoneType }.AsQueryable().TestAsync());

            _context.Expect(c => c.CommitAsync())
                .WhenCalled(inv => phoneType.RowVersion = new byte[] { 0x02 })
                .Return(Task.FromResult(1));

            var result = await _mediator.SendAsync(new UpdatePhoneType(phoneTypeData));
            Assert.AreEqual(1, result.Id);
            CollectionAssert.AreEqual(new byte[] { 0x02 }, result.RowVersion);

            Assert.AreEqual(phoneTypeData.Name, phoneType.Name);

            _context.VerifyAllExpectations();
        }
    }
}

namespace BibleTraining.Test.PhoneType
{
    using Api.PhoneType;
    using FluentValidation;

    public class UpdatePhoneTypeIntegrity : AbstractValidator<UpdatePhoneType>
    {
        public UpdatePhoneTypeIntegrity()
        {
            RuleFor(x => x.Resource)
                .NotNull()
                .SetValidator(new PhoneTypeDataIntegrity());
        }

        private class PhoneTypeDataIntegrity : AbstractValidator<PhoneTypeData>
        {
            public PhoneTypeDataIntegrity()
            {
                RuleFor(x => x.Id)
                    .NotNull();
                RuleFor(x => x.Name)
                    .NotEmpty();
            }
        }
    }
}

namespace BibleTraining.Test.PhoneType
{
    using Api.PhoneType;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class UpdatePhoneTypeIntegrityTests
    {
        private UpdatePhoneType updatePhoneType;
        private UpdatePhoneTypeIntegrity validator;

        [TestInitialize]
        public void TestInitialize()
        {
            updatePhoneType =  new UpdatePhoneType
            {
                Resource = new PhoneTypeData
                {
                    Id   = 1,
                    Name = "a"
                }
            };

            validator = new UpdatePhoneTypeIntegrity();
        }

        [TestMethod]
        public void IsValid()
        {
            var result = validator.Validate(updatePhoneType);
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void MustHaveName()
        {
            updatePhoneType.Resource.Name = string.Empty;
            var result = validator.Validate(updatePhoneType);
            Assert.IsFalse(result.IsValid);
        }
    }
}

namespace BibleTraining.Test.PhoneType
{
    using Api.PhoneType;
    using Improving.MediatR;

    public class RemovePhoneType : UpdateResource<PhoneTypeData, int?>
    {
        public RemovePhoneType()
        {
        }

        public RemovePhoneType(PhoneTypeData phoneType)
            : base(phoneType)
        {
        }
    }
}


namespace BibleTraining.Test.PhoneType
{
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Threading.Tasks;
    using Api.PhoneType;
    using Entities;
    using FizzWare.NBuilder;
    using Infrastructure;
    using Rhino.Mocks;


    [TestClass]
    public class RemovePhoneTypeTests : TestScenario
    {
        [TestMethod]
        public async Task ShouldRemovePhoneType()
        {
            var entity = new PhoneType
            {
                Id         = 1,
                Name       = "a",
                RowVersion = new byte[] { 0x01 }
            };

            var phoneTypeData = Builder<PhoneTypeData>.CreateNew()
                .With(pg => pg.Id = 1).And(c => c.RowVersion = new byte[] { 0x01 })
                .Build();

            _context.Expect(pg => pg.AsQueryable<PhoneType>())
                .Return(new[] { entity }.AsQueryable().TestAsync());

            _context.Expect(c => c.Remove(entity))
                .Return(entity);

            _context.Expect(c => c.CommitAsync())
                .Return(Task.FromResult(1));

            var result = await _mediator.SendAsync(new RemovePhoneType(phoneTypeData));
            Assert.AreEqual(1, result.Id);
            CollectionAssert.AreEqual(new byte[] { 0x01 }, result.RowVersion);

            _context.VerifyAllExpectations();
        }
    }
}

namespace BibleTraining.Test.PhoneType
{
    using Api.PhoneType;
    using Entities;
    using Improving.MediatR;
    using Improving.Highway.Data.Scope.Concurrency;

    [RelativeOrder(5), StopOnFailure]
    public class PhoneTypeConcurency : CheckConcurrency<PhoneType, PhoneTypeData>
    {
    }
}

namespace BibleTraining.Test.PhoneType
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Data.Entity.Core;
    using System.Linq;
    using Api.PhoneType;
    using Castle.MicroKernel.Registration;
    using Castle.Windsor;
    using Entities;
    using FizzWare.NBuilder;
    using Improving.MediatR;
    using Infrastructure;
    using Rhino.Mocks;

    [TestClass]
    public class PhoneTypeConcurrencyTests : TestScenario
    {
        private PhoneType _phoneType;

        protected override void BeforeContainer(IWindsorContainer container)
        {
            _phoneType = Builder<PhoneType>.CreateNew()
                .With(b => b.Id = 1)
                .And(b => b.RowVersion = new byte[] { 0x02 })
                .Build();
            container.Register(Component.For<PhoneType>().Instance(_phoneType));
        }

        [TestMethod]
        public void DetectsConcurrencyViolationOnUpdate()
        {
            var phoneType = Builder<PhoneTypeData>.CreateNew()
                .With(c => c.Id = 1).And(c => c.RowVersion = new byte[] { 0x01 })
                .Build();

            _context.Expect(c => c.AsQueryable<PhoneType>())
                .Return(new[] { _phoneType }.AsQueryable().TestAsync());

            var request = new UpdatePhoneType(phoneType);

            try
            {
                AssertNoValidationErrors<PhoneTypeConcurency, UpdateResource<PhoneTypeData, int?>>(request);
                Assert.Fail("Should have thrown OptimisticConcurrencyException");
            }
            catch (OptimisticConcurrencyException ex)
            {
                Assert.AreEqual(ex.Message,
                                $"Concurrency exception detected for {typeof(PhoneType).FullName} with id 1.");
            }
        }

        [TestMethod]
        public void DetectsConcurrencyViolationOnRemove()
        {
            var phoneType = Builder<PhoneTypeData>.CreateNew()
                .With(c => c.Id = 1).And(c => c.RowVersion = new byte[] { 0x01 })
                .Build();

            _context.Expect(c => c.AsQueryable<PhoneType>())
                .Return(new[] { _phoneType }.AsQueryable().TestAsync());

            var request = new RemovePhoneType(phoneType);

            try
            {
                AssertNoValidationErrors<PhoneTypeConcurency, UpdateResource<PhoneTypeData, int?>>(request);
                Assert.Fail("Should have thrown OptimisticConcurrencyException");
            }
            catch (OptimisticConcurrencyException ex)
            {
                Assert.AreEqual(ex.Message,
                                $"Concurrency exception detected for {typeof(PhoneType).FullName} with id 1.");
            }
        }
    }
}

namespace BibleTraining.Api.PhoneType 
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Improving.Highway.Data.Scope.Repository;
    using Entities;
    using Miruken.Mediate;
    using Queries;

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

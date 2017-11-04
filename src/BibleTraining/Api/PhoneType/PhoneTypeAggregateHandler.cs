namespace BibleTraining.Api.PhoneType
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Entities;
    using Improving.Highway.Data.Scope.Repository;
    using Miruken;
    using Miruken.Callback;
    using Miruken.Callback.Policy;
    using Miruken.Map;
    using Miruken.Mediate;
    using Queries;

    public class PhoneTypeAggregateHandler : PipelineHandler,
        IGlobalMiddleware<UpdatePhoneType, PhoneTypeData>,
        IGlobalMiddleware<RemovePhoneType, PhoneTypeData>
    {
        public int? Order { get; set; } = Stage.Validation - 1;

        private readonly IRepository<IBibleTrainingDomain> _repository;

        public PhoneTypeAggregateHandler(IRepository<IBibleTrainingDomain> repository)
        {
            _repository = repository;
        }

        public async Task<PhoneType> PhoneType(int? id, IHandler composer)
        {
            return await composer.Proxy<IStash>().GetOrPut(async () =>
                (await _repository.FindAsync(new GetPhoneTypesById(id)))
                    .FirstOrDefault());
        }

        public async Task<PhoneTypeData> Begin(int? id, IHandler composer, NextDelegate<Task<PhoneTypeData>> next)
        {
            using (var scope = _repository.Scopes.Create())
            {
                var phoneType = await PhoneType(id, composer);
                var result = await next();
                await scope.SaveChangesAsync();

                result.RowVersion = phoneType?.RowVersion;
                return result;
            }
        }

        [Mediates]
        public async Task<PhoneTypeData> Create(CreatePhoneType message, IHandler composer)
        {
            using(var scope = _repository.Scopes.Create())
            {
                var phoneType = composer.Proxy<IMapping>().Map<PhoneType>(message.Resource);
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

        [Mediates]
        public async Task<PhoneTypeResult> Get(GetPhoneTypes message, IHandler composer)
        {
            using(_repository.Scopes.CreateReadOnly())
            {
                var phoneTypes = (await _repository.FindAsync(new GetPhoneTypesById(message.Ids){
                    KeyProperties = message.KeyProperties
                })).Select(x => composer.Proxy<IMapping>().Map<PhoneTypeData>(x)).ToArray();

                return new PhoneTypeResult
                {
                    PhoneTypes = phoneTypes
                };
            }
        }

        public async Task<PhoneTypeData> Next(UpdatePhoneType request, MethodBinding method, IHandler composer, NextDelegate<Task<PhoneTypeData>> next)
        {
            return await Begin(request.Resource.Id, composer, next);
        }

        [Mediates]
        public async Task<PhoneTypeData> Update(UpdatePhoneType request, IHandler composer)
        {
            var phoneType = await PhoneType(request.Resource.Id, composer);
            composer.Proxy<IMapping>()
                .MapInto(request.Resource, phoneType);

            return new PhoneTypeData
            {
                Id = request.Resource.Id
            };
        }

        public async Task<PhoneTypeData> Next(RemovePhoneType request, MethodBinding method, IHandler composer, NextDelegate<Task<PhoneTypeData>> next)
        {
            return await Begin(request.Resource.Id, composer, next);
        }

        [Mediates]
        public async Task<PhoneTypeData> Remove(RemovePhoneType request, IHandler composer)
        {
            var phoneType = await PhoneType(request.Resource.Id, composer);
            _repository.Context.Remove(phoneType);

            return new PhoneTypeData
            {
                Id         = phoneType.Id,
                RowVersion = phoneType.RowVersion
            };
        }
    }
}

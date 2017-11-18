namespace BibleTraining.Api.AddressType
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

    public class AddressTypeAggregateHandler : PipelineHandler,
        IMiddleware<UpdateAddressType, AddressTypeData>,
        IMiddleware<RemoveAddressType, AddressTypeData>
    {
        public int? Order { get; set; } = Stage.Validation - 1;

        private readonly IRepository<IBibleTrainingDomain> _repository;

        public AddressTypeAggregateHandler(IRepository<IBibleTrainingDomain> repository)
        {
            _repository = repository;
        }

        public async Task<AddressType> AddressType(int? id, IHandler composer)
        {
            return await composer.Proxy<IStash>().GetOrPut(async () =>
                (await _repository.FindAsync(new GetAddressTypesById(id)))
                    .FirstOrDefault());
        }

        public async Task<AddressTypeData> Begin(int? id, IHandler composer, NextDelegate<Task<AddressTypeData>> next)
        {
            using (var scope = _repository.Scopes.Create())
            {
                var addressType = await AddressType(id, composer);
                var result = await next();
                await scope.SaveChangesAsync();

                result.RowVersion = addressType?.RowVersion;
                return result;
            }
        }

        [Mediates]
        public async Task<AddressTypeData> Create(CreateAddressType message, IHandler composer)
        {
            using(var scope = _repository.Scopes.Create())
            {
                var addressType = composer.Proxy<IMapping>().Map<AddressType>(message.Resource);

                _repository.Context.Add(addressType);

                var data = new AddressTypeData();

                await scope.SaveChangesAsync((dbScope, count) =>
                {
                    data.Id = addressType.Id;
                    data.RowVersion = addressType.RowVersion;
                });

                return data;
            }
        }

        [Mediates]
        public async Task<AddressTypeResult> Get(GetAddressTypes message, IHandler composer)
        {
            using(_repository.Scopes.CreateReadOnly())
            {
                var addressTypes = (await _repository.FindAsync(new GetAddressTypesById(message.Ids){
                    KeyProperties = message.KeyProperties
                })).Select(x => composer.Proxy<IMapping>().Map<AddressTypeData>(x)).ToArray();

                return new AddressTypeResult
                {
                    AddressTypes = addressTypes
                };
            }
        }

        public async Task<AddressTypeData> Next(UpdateAddressType request, MethodBinding method, IHandler composer, NextDelegate<Task<AddressTypeData>> next)
        {
            return await Begin(request.Resource.Id, composer, next);
        }

        [Mediates]
        public async Task<AddressTypeData> Update(UpdateAddressType request, IHandler composer)
        {
            var addressType = await AddressType(request.Resource.Id, composer);
            composer.Proxy<IMapping>()
                .MapInto(request.Resource, addressType);

            return new AddressTypeData
            {
                Id = request.Resource.Id
            };
        }

        public async Task<AddressTypeData> Next(RemoveAddressType request, MethodBinding method, IHandler composer, NextDelegate<Task<AddressTypeData>> next)
        {
            return await Begin(request.Resource.Id, composer, next);
        }

        [Mediates]
        public async Task<AddressTypeData> Remove(RemoveAddressType request, IHandler composer)
        {
            var addressType = await AddressType(request.Resource.Id, composer);
            _repository.Context.Remove(addressType);

            return new AddressTypeData
            {
                Id         = addressType.Id,
                RowVersion = addressType.RowVersion
            };
        }
    }
}

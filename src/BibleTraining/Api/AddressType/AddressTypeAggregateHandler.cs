namespace BibleTraining.Api.AddressType
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Entities;
    using Improving.Highway.Data.Scope.Repository;
    using Miruken.Callback;
    using Miruken.Callback.Policy;
    using Miruken.Map;
    using Miruken.Mediate;
    using Queries;

    public class AddressTypeAggregateHandler : PipelineHandler,
        IMiddleware<UpdateAddressType, AddressTypeData>,
        IMiddleware<RemoveAddressType, AddressTypeData>
    {
        private readonly IRepository<IBibleTrainingDomain> _repository;

        public AddressTypeAggregateHandler(
            IRepository<IBibleTrainingDomain> repository)
        {
            _repository = repository;
        }

        public int? Order { get; set; } = Stage.Validation - 1;

        public async Task<AddressTypeData> Begin(
            int? id, StashOf<AddressType> addressTypeStash,
            NextDelegate<Task<AddressTypeData>> next)
        {
            using (var scope = _repository.Scopes.Create())
            {
                var addressType = await AddressType(id, addressTypeStash);
                var result      = await next();
                await scope.SaveChangesAsync();

                result.RowVersion = addressType?.RowVersion;
                return result;
            }
        }

        [Mediates]
        public async Task<AddressTypeData> Create(
            CreateAddressType message, [Proxy]IMapping mapper)
        {
            using(var scope = _repository.Scopes.Create())
            {
                var addressType = mapper.Map<AddressType>(message.Resource);
                addressType.Created = DateTime.Now;
                _repository.Context.Add(addressType);

                var data = new AddressTypeData();

                await scope.SaveChangesAsync((dbScope, count) =>
                {
                    data.Id         = addressType.Id;
                    data.RowVersion = addressType.RowVersion;
                });

                return data;
            }
        }

        [Mediates]
        public async Task<AddressTypeResult> Get(
            GetAddressTypes message, [Proxy]IMapping mapper)
        {
            using (_repository.Scopes.CreateReadOnly())
            {
                var addressTypes = (await _repository.FindAsync(
                    new GetAddressTypesById(message.Ids)
                    {
                        KeyProperties = message.KeyProperties
                    }))
                    .Select(x => mapper.Map<AddressTypeData>(x))
                    .ToArray();

                return new AddressTypeResult
                {
                    AddressTypes = addressTypes
                };
            }
        }

        public async Task<AddressTypeData> Next(
            UpdateAddressType request, MethodBinding method,
            IHandler composer, NextDelegate<Task<AddressTypeData>> next)
        {
            return await Begin(request.Resource.Id,
                new StashOf<AddressType>(composer), next);
        }

        [Mediates]
        public async Task<AddressTypeData> Update(
            UpdateAddressType request, StashOf<AddressType> addressTypeStash,
            [Proxy]IMapping mapper)
        {
            var addressType = await AddressType(request.Resource.Id, addressTypeStash);
            mapper.MapInto(request.Resource, addressType);

            return new AddressTypeData
            {
                Id = request.Resource.Id
            };
        }

        public async Task<AddressTypeData> Next(
            RemoveAddressType request, MethodBinding method,
            IHandler composer, NextDelegate<Task<AddressTypeData>> next)
        {
            return await Begin(request.Resource.Id, 
                new StashOf<AddressType>(composer), next);
        }

        [Mediates]
        public async Task<AddressTypeData> Remove(
            RemoveAddressType request, StashOf<AddressType> addresTypeStash)
        {
            var addressType = await AddressType(request.Resource.Id, addresTypeStash);
            _repository.Context.Remove(addressType);

            return new AddressTypeData
            {
                Id         = addressType.Id,
                RowVersion = addressType.RowVersion
            };
        }

        protected Task<AddressType> AddressType(
            int? id, StashOf<AddressType> addressType)
        {
            return addressType.GetOrPut(async _ =>
                (await _repository.FindAsync(new GetAddressTypesById(id)))
                .FirstOrDefault());
        }
    }
}

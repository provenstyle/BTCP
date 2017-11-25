using System.Linq;

namespace BibleTraining.Api.Address
{
    using System;
    using System.Threading.Tasks;
    using Entities;
    using Improving.Highway.Data.Scope.Repository;
    using Miruken.Callback;
    using Miruken.Callback.Policy;
    using Miruken.Map;
    using Miruken.Mediate;
    using Queries;

    public class AddressAggregateHandler : PipelineHandler,
        IMiddleware<UpdateAddress, AddressData>,
        IMiddleware<RemoveAddress, AddressData>
    {
        private readonly IRepository<IBibleTrainingDomain> _repository;

        public AddressAggregateHandler(
            IRepository<IBibleTrainingDomain> repository)
        {
            _repository = repository;
        }

        public int? Order { get; set; } = Stage.Validation - 1;

        #region Create Address

        [Mediates]
        public async Task<AddressData> Create(
            CreateAddress message, StashOf<Address> addressStash, 
            [Optional]Person person, [Optional]AddressType addressType,
            [Proxy]IMapping mapper)
        {
            using(var scope = _repository.Scopes.Create())
            {
                var addressData = message.Resource;
                var address     = addressStash.Value =
                   mapper.Map<Address>(addressData);
                _repository.Context.Add(address);

                if (addressData.PersonId.HasValue)
                    address.PersonId = addressData.PersonId.Value;
                else
                    address.Person = person;

                if (addressData.AddressTypeId.HasValue)
                    address.AddressTypeId = addressData.AddressTypeId.Value;
                else
                    address.AddressType = addressType;

                var data = new AddressData();
                await scope.SaveChangesAsync((dbScope, count) =>
                 {
                     data.Id         = address.Id;
                     data.RowVersion = address.RowVersion;
                 });

                return data;
            }
        }

        #endregion

        #region Get Address

        [Mediates]
        public async Task<AddressResult> Get(
            GetAddresses message, [Proxy]IMapping mapper)
        {
            using(_repository.Scopes.CreateReadOnly())
            {
                var addresses = (await _repository.FindAsync(
                    new GetAddressesById(message.Ids)
                    {
                        KeyProperties = message.KeyProperties
                    }))
                    .Select(x => mapper.Map<AddressData>(x))
                    .ToArray();

                return new AddressResult
                {
                    Addresses = addresses
                };
            }
        }

        #endregion

        #region Update Address

        public async Task<AddressData> Next(
            UpdateAddress request, MethodBinding method, 
            IHandler composer, NextDelegate<Task<AddressData>> next)
        {
            return await Begin(request.Resource.Id, 
                new StashOf<Address>(composer), next);
        }

        [Mediates]
        public async Task<AddressData> Update(
            UpdateAddress request, [Proxy]IMapping mapper,
            StashOf<Address> addressStash)
        {
            var address = await Address(request.Resource.Id, addressStash);
            mapper.MapInto(request.Resource, address);

            return new AddressData
            {
                Id = request.Resource.Id
            };
        }

        #endregion

        #region Remove Address

        public async Task<AddressData> Next(
            RemoveAddress request, MethodBinding method, 
            IHandler composer, NextDelegate<Task<AddressData>> next)
        {
            return await Begin(request.Resource.Id, 
                new StashOf<Address>(composer), next);
        }

        [Mediates]
        public async Task<AddressData> Remove(
            RemoveAddress request, StashOf<Address> addressStash)
        {
            var address = await Address(request.Resource.Id, addressStash);
            _repository.Context.Remove(address);

            return new AddressData
            {
                Id         = address.Id,
                RowVersion = address.RowVersion
            };
        }

        #endregion

        protected async Task<AddressData> Begin(
            int? id, StashOf<Address> addressStash,
            NextDelegate<Task<AddressData>> next)
        {
            using (var scope = _repository.Scopes.Create())
            {
                var address = await Address(id, addressStash);
                var result = await next();
                await scope.SaveChangesAsync();
                result.RowVersion = address?.RowVersion;
                return result;
            }
        }
        protected Task<Address> Address(int? id, StashOf<Address> address)
        {
            return address.GetOrPut(async _ =>
                (await _repository.FindAsync(new GetAddressesById(id)))
                .FirstOrDefault());
        }
    }
}
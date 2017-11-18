namespace BibleTraining.Api.Address
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

    public class AddressAggregateHandler : PipelineHandler,
        IMiddleware<UpdateAddress, AddressData>,
        IMiddleware<RemoveAddress, AddressData>
    {
        public int? Order { get; set; } = Stage.Validation - 1;

        private readonly IRepository<IBibleTrainingDomain> _repository;

        public AddressAggregateHandler(IRepository<IBibleTrainingDomain> repository)
        {
            _repository = repository;
        }

        public async Task<Address> Address(int? id, IHandler composer)
        {
            return await composer.Proxy<IStash>().GetOrPut(async () =>
                (await _repository.FindAsync(new GetAddressesById(id)))
                    .FirstOrDefault());
        }

        public async Task<AddressData> Begin(int? id, IHandler composer, NextDelegate<Task<AddressData>> next)
        {
            using (var scope = _repository.Scopes.Create())
            {
                var address = await Address(id, composer);
                var result = await next();
                await scope.SaveChangesAsync();

                result.RowVersion = address?.RowVersion;
                return result;
            }
        }

        #region Create Address

        [Mediates]
        public async Task<AddressData> Create(CreateAddress message, IHandler composer)
        {
            using(var scope = _repository.Scopes.Create())
            {
                var addressData = message.Resource;

                var address = composer.Proxy<IMapping>() .Map<Address>(addressData);
                _repository.Context.Add(address);
                composer.Proxy<IStash>().Put(address);

                if (addressData.PersonId.HasValue)
                    address.PersonId = addressData.PersonId.Value;
                else
                    address.Person = composer.Proxy<IStash>().TryGet<Person>();

                if (addressData.AddressTypeId.HasValue)
                    address.AddressTypeId = addressData.AddressTypeId.Value;
                else
                    address.AddressType = composer.Proxy<IStash>().TryGet<AddressType>();

                var data = new AddressData();
                await scope.SaveChangesAsync((dbScope, count) =>
                 {
                     data.Id = address.Id;
                     data.RowVersion = address.RowVersion;
                 });

                return data;
            }
        }

        #endregion

        #region Get Address

        [Mediates]
        public async Task<AddressResult> Get(GetAddresses message, IHandler composer)
        {
            using(_repository.Scopes.CreateReadOnly())
            {
                var addresses = (await _repository.FindAsync(new GetAddressesById(message.Ids){
                    KeyProperties = message.KeyProperties
                })).Select(x => composer.Proxy<IMapping>().Map<AddressData>(x)).ToArray();

                return new AddressResult
                {
                    Addresses = addresses
                };
            }
        }

        #endregion

        #region Update Address

        public async Task<AddressData> Next(UpdateAddress request, MethodBinding method, IHandler composer, NextDelegate<Task<AddressData>> next)
        {
            return await Begin(request.Resource.Id, composer, next);
        }

        [Mediates]
        public async Task<AddressData> Update(UpdateAddress request, IHandler composer)
        {
            var address = await Address(request.Resource.Id, composer);
            composer.Proxy<IMapping>()
                .MapInto(request.Resource, address);

            return new AddressData
            {
                Id = request.Resource.Id
            };
        }

        #endregion

        #region Remove Address

        public async Task<AddressData> Next(RemoveAddress request, MethodBinding method, IHandler composer, NextDelegate<Task<AddressData>> next)
        {
            return await Begin(request.Resource.Id, composer, next);
        }

        [Mediates]
        public async Task<AddressData> Remove(RemoveAddress request, IHandler composer)
        {
            var address = await Address(request.Resource.Id, composer);
            _repository.Context.Remove(address);

            return new AddressData
            {
                Id         = address.Id,
                RowVersion = address.RowVersion
            };
        }

        #endregion
    }
}
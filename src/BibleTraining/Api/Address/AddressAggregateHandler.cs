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

        public Address Address { get; set; }

        public AddressAggregateHandler(IRepository<IBibleTrainingDomain> repository)
        {
            _repository = repository;
        }

        #region Create Address

        [Mediates]
        public async Task<AddressData> Handle(CreateAddress message, IHandler composer)
        {
            using(var scope = _repository.Scopes.Create())
            {
                var address = composer.Proxy<IMapping>()
                    .Map<Address>(message.Resource);
                address.Created = DateTime.Now;

                _repository.Context.Add(address);

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
        public async Task<AddressResult> Handle(GetAddresses message, IHandler composer)
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
            using (var scope = _repository.Scopes.Create())
            {
                var resource = request.Resource;
                if (Address == null && resource != null)
                {
                    Address = (await _repository
                                   .FindAsync(new GetAddressesById(resource.Id)))
                        .FirstOrDefault();
                    composer.Proxy<IStash>().Put(Address);
                }

                var result = await next();
                await scope.SaveChangesAsync();

                result.RowVersion = Address?.RowVersion;
                return result;
            }
        }

        [Mediates]
        public Task<AddressData> Handle(UpdateAddress request, IHandler composer)
        {
            composer.Proxy<IMapping>()
                .MapInto(request.Resource, Address);

            return Task.FromResult(new AddressData
            {
                Id = request.Resource.Id
            });
        }

        #endregion

        #region Remove Address

        public async Task<AddressData> Next(RemoveAddress request, MethodBinding method, IHandler composer, NextDelegate<Task<AddressData>> next)
        {
            using (var scope = _repository.Scopes.Create())
            {
                var resource = request.Resource;
                if (Address == null && resource != null)
                {
                    Address = (await _repository
                        .FindAsync(new GetAddressesById(resource.Id)))
                        .FirstOrDefault();
                    composer.Proxy<IStash>().Put(Address);
                }

                var result = await next();
                await scope.SaveChangesAsync();
                return result;
            }
        }

        [Mediates]
        public Task<AddressData> Handle(RemoveAddress request)
        {
            _repository.Context.Remove(Address);

            return Task.FromResult(new AddressData
            {
                Id         = Address.Id,
                RowVersion = Address.RowVersion
            });
        }

        #endregion

    }
}
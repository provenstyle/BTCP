namespace BibleTraining.Api.Address
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Entities;
    using Improving.Highway.Data.Scope.Repository;
    using Improving.MediatR;
    using Improving.MediatR.Pipeline;
    using MediatR;
    using Queries;
    using Test._CodeGeneration;

    [RelativeOrder(Stage.Validation - 1)]
    public class AddressAggregateHandler :
        IAsyncRequestHandler<CreateAddress, AddressData>,
        IAsyncRequestHandler<GetAddresses, AddressResult>,
        IAsyncRequestHandler<UpdateAddress, AddressData>,
        IRequestMiddleware<UpdateAddress, AddressData>,
        IAsyncRequestHandler<RemoveAddress, AddressData>,
        IRequestMiddleware<RemoveAddress, AddressData>
    {
        private readonly IRepository<IBibleTrainingDomain> _repository;

        public Address Address { get; set; }

        public AddressAggregateHandler(IRepository<IBibleTrainingDomain> repository)
        {
            _repository = repository;
        }

        #region Create Address

        public async Task<AddressData> Handle(CreateAddress message)
        {
            using(var scope = _repository.Scopes.Create())
            {
                var address = new Address().Map(message.Resource);
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

        public async Task<AddressResult> Handle(GetAddresses message)
        {
            using(_repository.Scopes.CreateReadOnly())
            {
                var addresses = (await _repository.FindAsync(new GetAddressesById(message.Ids){
                    KeyProperties = message.KeyProperties
                })).Select(x => new AddressData().Map(x)).ToArray();

                return new AddressResult
                {
                    Addresses = addresses
                };
            }
        }

        #endregion

        #region Update Address

        public async Task<AddressData> Apply(UpdateAddress request, Func<UpdateAddress, Task<AddressData>> next)
        {
            using (var scope = _repository.Scopes.Create())
            {
                var resource = request.Resource;
                if (Address == null && resource != null)
                {
                    Address = (await _repository
                                   .FindAsync(new GetAddressesById(resource.Id)))
                        .FirstOrDefault();
                    Env.Use(Address);
                }

                var result = await next(request);
                await scope.SaveChangesAsync();

                result.RowVersion = Address?.RowVersion;
                return result;
            }
        }

        public Task<AddressData> Handle(UpdateAddress request)
        {
            Address.Map(request.Resource);

            return Task.FromResult(new AddressData
            {
                Id = request.Resource.Id
            });
        }

        #endregion

        #region Remove Address

        public async Task<AddressData> Apply(
            RemoveAddress request, Func<RemoveAddress, Task<AddressData>> next)
        {
            using (var scope = _repository.Scopes.Create())
            {
                var resource = request.Resource;
                if (Address == null && resource != null)
                {
                    Address = (await _repository
                                   .FindAsync(new GetAddressesById(resource.Id)))
                        .FirstOrDefault();
                    Env.Use(Address);
                }

                var result = await next(request);
                await scope.SaveChangesAsync();
                return result;
            }
        }

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
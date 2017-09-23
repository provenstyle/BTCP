namespace BibleTraining.Api.AddressType
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Entities;
    using Improving.Highway.Data.Scope.Repository;
    using Miruken;
    using Miruken.Callback;
    using Miruken.Map;
    using Miruken.Mediate;
    using Queries;

    public class AddressTypeAggregateHandler : Handler
    {
        private readonly IRepository<IBibleTrainingDomain> _repository;

        public AddressTypeAggregateHandler(IRepository<IBibleTrainingDomain> repository)
        {
            _repository = repository;
        }

        public Task<AddressType> GetAddressType(AddressTypeData resource, IHandler composer)
        {
            return composer.Proxy<IStash>().GetOrPut(async () =>
             {
                using (_repository.Scopes.Create())
                {
                    return (await _repository.FindAsync(new GetAddressTypesById(resource.Id)))
                            .FirstOrDefault();
                }
             });
        }

        [Handles]
        public async Task<AddressTypeData> Handle(CreateAddressType message, IHandler composer)
        {
            using(var scope = _repository.Scopes.Create())
            {
                var addressType = composer.Proxy<IMapping>()
                    .Map<AddressType>(message.Resource);

                addressType.Created = DateTime.Now;

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

        [Handles]
        public async Task<AddressTypeResult> Handle(GetAddressTypes message, IHandler composer)
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

        [Handles]
        public Task<AddressTypeData> Handle(UpdateAddressType request, IHandler composer)
        {
            composer.Proxy<IMapping>()
                .MapInto(request.Resource, GetAddressType(request.Resource, composer));

            return Task.FromResult(new AddressTypeData
            {
                Id = request.Resource.Id
            });
        }

        [Handles]
        public async Task<AddressTypeData> Handle(RemoveAddressType request, IHandler composer)
        {
            var entity = await GetAddressType(request.Resource, composer);
            _repository.Context.Remove(entity);

            return new AddressTypeData
            {
                Id         = entity.Id,
                RowVersion = entity.RowVersion
            };
        }
    }
}
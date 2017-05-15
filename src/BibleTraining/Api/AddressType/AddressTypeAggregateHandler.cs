namespace BibleTraining.Api.AddressType
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

    [RelativeOrder(Stage.Validation - 1)]
    public class AddressTypeAggregateHandler :
        IAsyncRequestHandler<CreateAddressType, AddressTypeData>,
        IAsyncRequestHandler<GetAddressTypes, AddressTypeResult>,
        IAsyncRequestHandler<UpdateAddressType, AddressTypeData>,
        IRequestMiddleware<UpdateAddressType, AddressTypeData>,
        IAsyncRequestHandler<RemoveAddressType, AddressTypeData>,
        IRequestMiddleware<RemoveAddressType, AddressTypeData>
    {
        private readonly IRepository<IBibleTrainingDomain> _repository;

        public AddressType AddressType { get; set; }

        public AddressTypeAggregateHandler(IRepository<IBibleTrainingDomain> repository)
        {
            _repository = repository;
        }

        #region Create AddressType

        public async Task<AddressTypeData> Handle(CreateAddressType message)
        {
            using(var scope = _repository.Scopes.Create())
            {
                var addressType = new AddressType().Map(message.Resource);
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

        #endregion

        #region Get AddressType

        public async Task<AddressTypeResult> Handle(GetAddressTypes message)
        {
            using(_repository.Scopes.CreateReadOnly())
            {
                var addressTypes = (await _repository.FindAsync(new GetAddressTypesById(message.Ids){
                    KeyProperties = message.KeyProperties
                })).Select(x => new AddressTypeData().Map(x)).ToArray();

                return new AddressTypeResult
                {
                    AddressTypes = addressTypes
                };
            }
        }

        #endregion

        #region Update AddressType

        public async Task<AddressTypeData> Apply(UpdateAddressType request, Func<UpdateAddressType, Task<AddressTypeData>> next)
        {
            using (var scope = _repository.Scopes.Create())
            {
                var resource = request.Resource;
                if (AddressType == null && resource != null)
                {
                    AddressType = (await _repository
                                       .FindAsync(new GetAddressTypesById(resource.Id)))
                        .FirstOrDefault();
                    Env.Use(AddressType);
                }

                var result = await next(request);
                await scope.SaveChangesAsync();

                result.RowVersion = AddressType?.RowVersion;
                return result;
            }
        }

        public Task<AddressTypeData> Handle(UpdateAddressType request)
        {
            AddressType.Map(request.Resource);

            return Task.FromResult(new AddressTypeData
            {
                Id = request.Resource.Id
            });
        }

        #endregion

        #region Remove AddressType

        public async Task<AddressTypeData> Apply(
            RemoveAddressType request, Func<RemoveAddressType, Task<AddressTypeData>> next)
        {
            using (var scope = _repository.Scopes.Create())
            {
                var resource = request.Resource;
                if (AddressType == null && resource != null)
                {
                    AddressType = (await _repository
                                       .FindAsync(new GetAddressTypesById(resource.Id)))
                        .FirstOrDefault();
                    Env.Use(AddressType);
                }

                var result = await next(request);
                await scope.SaveChangesAsync();
                return result;
            }
        }

        public Task<AddressTypeData> Handle(RemoveAddressType request)
        {
            _repository.Context.Remove(AddressType);

            return Task.FromResult(new AddressTypeData
            {
                Id         = AddressType.Id,
                RowVersion = AddressType.RowVersion
            });
        }

        #endregion

    }
}
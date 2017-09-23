namespace BibleTraining.Api.Phone
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Entities;
    using Improving.Highway.Data.Scope.Repository;
    using Miruken.Mediate;
    using Queries;

    [RelativeOrder(Stage.Validation - 1)]
    public class PhoneAggregateHandler :
        IAsyncRequestHandler<CreatePhone, PhoneData>,
        IAsyncRequestHandler<GetPhones, PhoneResult>,
        IAsyncRequestHandler<UpdatePhone, PhoneData>,
        IRequestMiddleware<UpdatePhone, PhoneData>,
        IAsyncRequestHandler<RemovePhone, PhoneData>,
        IRequestMiddleware<RemovePhone, PhoneData>
    {
        private readonly IRepository<IBibleTrainingDomain> _repository;

        public Phone Phone { get; set; }

        public PhoneAggregateHandler(IRepository<IBibleTrainingDomain> repository)
        {
            _repository = repository;
        }

        #region Create Phone

        public async Task<PhoneData> Handle(CreatePhone message)
        {
            using(var scope = _repository.Scopes.Create())
            {
                var phone = new Phone().Map(message.Resource);
                phone.Created = DateTime.Now;

                _repository.Context.Add(phone);

                var data = new PhoneData();

                await scope.SaveChangesAsync((dbScope, count) =>
                                                 {
                                                     data.Id = phone.Id;
                                                     data.RowVersion = phone.RowVersion;
                                                 });

                return data;
            }
        }

        #endregion

        #region Get Phone

        public async Task<PhoneResult> Handle(GetPhones message)
        {
            using(_repository.Scopes.CreateReadOnly())
            {
                var phones = (await _repository.FindAsync(new GetPhonesById(message.Ids){
                    KeyProperties = message.KeyProperties
                })).Select(x => new PhoneData().Map(x)).ToArray();

                return new PhoneResult
                {
                    Phones = phones
                };
            }
        }

        #endregion

        #region Update Phone

        public async Task<PhoneData> Apply(UpdatePhone request, Func<UpdatePhone, Task<PhoneData>> next)
        {
            using (var scope = _repository.Scopes.Create())
            {
                var resource = request.Resource;
                if (Phone == null && resource != null)
                {
                    Phone = (await _repository
                                 .FindAsync(new GetPhonesById(resource.Id)))
                        .FirstOrDefault();
                    Env.Use(Phone);
                }

                var result = await next(request);
                await scope.SaveChangesAsync();

                result.RowVersion = Phone?.RowVersion;
                return result;
            }
        }

        public Task<PhoneData> Handle(UpdatePhone request)
        {
            Phone.Map(request.Resource);

            return Task.FromResult(new PhoneData
            {
                Id = request.Resource.Id
            });
        }

        #endregion

        #region Remove Phone

        public async Task<PhoneData> Apply(
            RemovePhone request, Func<RemovePhone, Task<PhoneData>> next)
        {
            using (var scope = _repository.Scopes.Create())
            {
                var resource = request.Resource;
                if (Phone == null && resource != null)
                {
                    Phone = (await _repository
                                 .FindAsync(new GetPhonesById(resource.Id)))
                        .FirstOrDefault();
                    Env.Use(Phone);
                }

                var result = await next(request);
                await scope.SaveChangesAsync();
                return result;
            }
        }

        public Task<PhoneData> Handle(RemovePhone request)
        {
            _repository.Context.Remove(Phone);

            return Task.FromResult(new PhoneData
            {
                Id         = Phone.Id,
                RowVersion = Phone.RowVersion
            });
        }

        #endregion

    }
}
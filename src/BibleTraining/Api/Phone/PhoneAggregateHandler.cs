namespace BibleTraining.Api.Phone
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

    public class PhoneAggregateHandler : PipelineHandler,
        IMiddleware<UpdatePhone, PhoneData>,
        IMiddleware<RemovePhone, PhoneData>
    {
        private readonly IRepository<IBibleTrainingDomain> _repository;

        public PhoneAggregateHandler(
            IRepository<IBibleTrainingDomain> repository)
        {
            _repository = repository;
        }

        public int? Order { get; set; } = Stage.Validation - 1;

        public async Task<PhoneData> Begin(
            int? id, IHandler composer, NextDelegate<Task<PhoneData>> next)
        {
            using (var scope = _repository.Scopes.Create())
            {
                var phone  = await Phone(id, composer);
                var result = await next();
                await scope.SaveChangesAsync();

                result.RowVersion = phone?.RowVersion;
                return result;
            }
        }

        [Mediates]
        public async Task<PhoneData> Create(
            CreatePhone message, IHandler composer,
            StashOf<Phone> phoneStash, [Optional]Person person,
            [Optional]PhoneType phoneType)
        {
            using(var scope = _repository.Scopes.Create())
            {
                var phoneData = message.Resource;

                var phone = phoneStash.Value =
                    composer.Proxy<IMapping>().Map<Phone>(message.Resource);
                phone.Created = DateTime.Now;
                _repository.Context.Add(phone);

                if (phoneData.PersonId.HasValue)
                    phone.PersonId = phoneData.PersonId.Value;
                else
                    phone.Person = person;

                if (phoneData.PhoneTypeId.HasValue)
                    phone.PhoneTypeId = phoneData.PhoneTypeId.Value;
                else
                    phone.PhoneType = phoneType;

                var data = new PhoneData();
                await scope.SaveChangesAsync((dbScope, count) =>
                {
                    data.Id         = phone.Id;
                    data.RowVersion = phone.RowVersion;
                });

                return data;
            }
        }

        [Mediates]
        public async Task<PhoneResult> Get(
            GetPhones message, IHandler composer)
        {
            using(_repository.Scopes.CreateReadOnly())
            {
                var phones = (await _repository.FindAsync(
                    new GetPhonesById(message.Ids)
                    {
                        KeyProperties = message.KeyProperties
                    }))
                    .Select(x => composer.Proxy<IMapping>().Map<PhoneData>(x))
                    .ToArray();

                return new PhoneResult
                {
                    Phones = phones
                };
            }
        }

        public async Task<PhoneData> Next(
            UpdatePhone request, MethodBinding method, 
            IHandler composer, NextDelegate<Task<PhoneData>> next)
        {
            return await Begin(request.Resource.Id, composer, next);
        }

        [Mediates]
        public async Task<PhoneData> Update(UpdatePhone request, IHandler composer)
        {
            var phone = await Phone(request.Resource.Id, composer);
            composer.Proxy<IMapping>()
                .MapInto(request.Resource, phone);

            return new PhoneData
            {
                Id = request.Resource.Id
            };
        }

        public async Task<PhoneData> Next(
            RemovePhone request, MethodBinding method,
            IHandler composer, NextDelegate<Task<PhoneData>> next)
        {
            return await Begin(request.Resource.Id, composer, next);
        }

        [Mediates]
        public async Task<PhoneData> Remove(RemovePhone request, IHandler composer)
        {
            var phone = await Phone(request.Resource.Id, composer);
            _repository.Context.Remove(phone);

            return new PhoneData
            {
                Id         = phone.Id,
                RowVersion = phone.RowVersion
            };
        }

        protected async Task<Phone> Phone(int? id, IHandler composer)
        {
            return await composer.Proxy<IStash>().GetOrPut(async () =>
                (await _repository.FindAsync(new GetPhonesById(id)))
                .FirstOrDefault());
        }
    }
}

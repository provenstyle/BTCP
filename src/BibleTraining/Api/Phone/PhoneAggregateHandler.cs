namespace BibleTraining.Api.Phone
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

        [Mediates]
        public async Task<PhoneData> Create(
            CreatePhone message, StashOf<Phone> phoneStash,
            [Optional]Person person, [Optional]PhoneType phoneType,
            [Proxy]IMapping mapper)
        {
            using(var scope = _repository.Scopes.Create())
            {
                var phoneData = message.Resource;

                var phone = phoneStash.Value =
                    mapper.Map<Phone>(message.Resource);
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
            GetPhones message, [Proxy]IMapping mapper)
        {
            using(_repository.Scopes.CreateReadOnly())
            {
                var phones = (await _repository.FindAsync(
                    new GetPhonesById(message.Ids)
                    {
                        KeyProperties = message.KeyProperties
                    }))
                    .Select(x => mapper.Map<PhoneData>(x))
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
            return await Begin(request.Resource.Id,
                new StashOf<Phone>(composer), next);
        }

        [Mediates]
        public async Task<PhoneData> Update(
            UpdatePhone request, StashOf<Phone> phoneStash,
            [Proxy]IMapping mapper)
        {
            var phone = await Phone(request.Resource.Id, phoneStash);
            mapper.MapInto(request.Resource, phone);

            return new PhoneData
            {
                Id = request.Resource.Id
            };
        }

        public async Task<PhoneData> Next(
            RemovePhone request, MethodBinding method,
            IHandler composer, NextDelegate<Task<PhoneData>> next)
        {
            return await Begin(request.Resource.Id, 
                new StashOf<Phone>(composer), next);
        }

        [Mediates]
        public async Task<PhoneData> Remove(
            RemovePhone request, StashOf<Phone> phoneStash)
        {
            var phone = await Phone(request.Resource.Id, phoneStash);
            _repository.Context.Remove(phone);

            return new PhoneData
            {
                Id         = phone.Id,
                RowVersion = phone.RowVersion
            };
        }

        protected async Task<PhoneData> Begin(
            int? id, StashOf<Phone> phoneStash, NextDelegate<Task<PhoneData>> next)
        {
            using (var scope = _repository.Scopes.Create())
            {
                var phone = await Phone(id, phoneStash);
                var result = await next();
                await scope.SaveChangesAsync();

                result.RowVersion = phone?.RowVersion;
                return result;
            }
        }

        protected Task<Phone> Phone(int? id, StashOf<Phone> phoneType)
        {
            return phoneType.GetOrPut(async _ =>
                (await _repository.FindAsync(new GetPhonesById(id)))
                .FirstOrDefault());
        }
    }
}

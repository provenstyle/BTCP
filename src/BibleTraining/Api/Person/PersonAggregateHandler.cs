namespace BibleTraining.Api.Person
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Address;
    using Email;
    using Entities;
    using Improving.Highway.Data.Scope.Repository;
    using Miruken.Callback;
    using Miruken.Callback.Policy;
    using Miruken.Map;
    using Miruken.Mediate;
    using Phone;
    using Queries;

    public class PersonAggregateHandler : PipelineHandler,
        IGlobalMiddleware<UpdatePerson, PersonData>,
        IGlobalMiddleware<RemovePerson, PersonData>
    {
        private readonly IRepository<IBibleTrainingDomain> _repository;

        public  PersonAggregateHandler(
            IRepository<IBibleTrainingDomain> repository)
        {
            _repository = repository;
        }

        public int? Order { get; set; } = Stage.Validation - 1;

        public Task<Person> Person(int? id, StashOf<Person> personStash)
        {
            return personStash.GetOrPut(async _ =>
               (await _repository.FindAsync(new GetPeopleById(id)
               {
                   IncludeEmails    = true,
                   IncludeAddresses = true,
                   IncludePhones    = true
               })).FirstOrDefault());
        }

        public async Task<PersonData> Begin(
            int? id, StashOf<Person> personStash,
            NextDelegate<Task<PersonData>> next)
        {
            using (var scope = _repository.Scopes.Create())
            {
                var person = await Person(id, personStash);
                var result = await next();
                await scope.SaveChangesAsync();

                result.RowVersion = person?.RowVersion;
                return result;
            }
        }

        [Mediates]
        public async Task<PersonData> Create(
            CreatePerson request, StashOf<Person> personStash,
            [Proxy]IMapping mapper, IHandler composer)
        {
            using(var scope = _repository.Scopes.Create())
            {
                var person = personStash.Value =
                    mapper.Map<Person>(request.Resource);
                _repository.Context.Add(person);

                var relationships = new List<object>();

                var emails = request.Resource.Emails;
                if (emails != null)
                {
                    var adds = emails.Where(x => !x.Id.HasValue).ToArray();
                    relationships.AddRange(adds.Select(add => new CreateEmail(add)));
                }

                var addresses = request.Resource.Addresses;
                if (addresses != null)
                {
                    var adds = addresses.Where(x => !x.Id.HasValue).ToArray();
                    relationships.AddRange(adds.Select(add => new CreateAddress(add)));
                }

                var phones = request.Resource.Phones;
                if (phones != null)
                {
                    var adds = phones.Where(x => !x.Id.HasValue).ToArray();
                    relationships.AddRange(adds.Select(add => new CreatePhone(add)));
                }

                foreach (var relationship in relationships)
                    await composer.Send(relationship);

                var data = new PersonData();
                await scope.SaveChangesAsync((dbScope, count) =>
                {
                    data.Id         = person.Id;
                    data.RowVersion = person.RowVersion;
                });

                return data;
            }
        }

        [Mediates]
        public async Task<PersonResult> Get(GetPeople message, [Proxy]IMapping mapper)
        {
            using (_repository.Scopes.CreateReadOnly())
            {
                var people = (await _repository
                    .FindAsync(new GetPeopleById(message.Ids)
                      {
                         IncludePhones    = message.IncludePhones,
                         IncludeEmails    = message.IncludeEmails,
                         IncludeAddresses = message.IncludeAddresses
                      }))
                    .Select(x => mapper.Map<PersonData>(x))
                    .ToArray();

                return new PersonResult
                {
                    People = people
                };
            }
        }

        public async Task<PersonData> Next(
            UpdatePerson request, MethodBinding method,
            IHandler composer, NextDelegate<Task<PersonData>> next)
        {
            return await Begin(request.Resource.Id,
                new StashOf<Person>(composer), next);
        }

        [Mediates]
        public async Task<PersonData> Update(
            UpdatePerson request, StashOf<Person> personStash,
            [Proxy]IMapping mapper, IHandler composer)
        {
            var person = await Person(request.Resource.Id, personStash);
            mapper.MapInto(request.Resource, person);
            personStash.Value = person;

            var relationships = new List<object>();

            var emails = request.Resource.Emails;
            if (emails != null)
            {
                var adds = emails.Where(x => !x.Id.HasValue).ToArray();
                var updates = emails.Where(x => x.Id.HasValue).ToArray();
                var updateIds = updates.Select(x => x.Id).ToArray();
                var removes = person.Emails?
                    .Where(x => !updateIds.Contains(x.Id))
                    .Select(x => mapper.Map<EmailData>(x))
                    .ToArray();

                relationships.AddRange(adds.Select(add => new CreateEmail(add)));
                relationships.AddRange(updates.Select(update => new UpdateEmail(update)));

                if (removes != null)
                    relationships.AddRange(removes.Select(remove => new RemoveEmail(remove)));
            }

            var addresses = request.Resource.Addresses;
            if (addresses != null)
            {
                var adds = addresses.Where(x => !x.Id.HasValue).ToArray();
                var updates = addresses.Where(x => x.Id.HasValue).ToArray();
                var updateIds = updates.Select(x => x.Id).ToArray();
                var removes = person.Addresses?
                    .Where(x => !updateIds.Contains(x.Id))
                    .Select(x => mapper.Map<AddressData>(x))
                    .ToArray();

                relationships.AddRange(adds.Select(add => new CreateAddress(add)));
                relationships.AddRange(updates.Select(update => new UpdateAddress(update)));

                if (removes != null)
                    relationships.AddRange(removes.Select(remove => new RemoveAddress(remove)));
            }

            var phones = request.Resource.Phones;
            if (phones != null)
            {
                var adds = phones.Where(x => !x.Id.HasValue).ToArray();
                var updates = phones.Where(x => x.Id.HasValue).ToArray();
                var updateIds = updates.Select(x => x.Id).ToArray();
                var removes = person.Phones?
                    .Where(x => !updateIds.Contains(x.Id))
                    .Select(x => mapper.Map<PhoneData>(x))
                    .ToArray();

                relationships.AddRange(adds.Select(add => new CreatePhone(add)));
                relationships.AddRange(updates.Select(update => new UpdatePhone(update)));

                if (removes != null)
                    relationships.AddRange(removes.Select(remove => new RemovePhone(remove)));
            }

            foreach (var relationship in relationships)
                await composer.Send(relationship);

            return new PersonData
            {
                Id = request.Resource.Id
            };
        }

        public async Task<PersonData> Next(
            RemovePerson request, MethodBinding method,
            IHandler composer, NextDelegate<Task<PersonData>> next)
        {
            return await Begin(request.Resource.Id,
                new StashOf<Person>(composer), next);
        }

        [Mediates]
        public async Task<PersonData> Remove(
            RemovePerson request, StashOf<Person> personStash)
        {
            var person = await Person(request.Resource.Id, personStash);
            _repository.Context.Remove(person);

            return new PersonData
            {
                Id         = person.Id,
                RowVersion = person.RowVersion
            };
        }
    }
}

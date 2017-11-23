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
    using Miruken.Map;
    using Miruken.Mediate;
    using Phone;
    using Queries;

    public class PersonAggregateHandler : PersonAggregateHandlerBase
    {
        public PersonAggregateHandler(
            IRepository<IBibleTrainingDomain> repository) 
            : base(repository)
        {
        }

        protected override Task<object[]> GetUpdateRelationships(
            UpdatePerson request, Person person, [Proxy]IMapping mapper)
        {
            var relationships = new List<object>();

            var emails = request.Resource.Emails;
            if (emails != null)
            {
                var adds      = emails.Where(x => !x.Id.HasValue).ToArray();
                var updates   = emails.Where(x => x.Id.HasValue).ToArray();
                var updateIds = updates.Select(x => x.Id).ToArray();
                var removes   = person.Emails?
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
                var adds      = addresses.Where(x => !x.Id.HasValue).ToArray();
                var updates   = addresses.Where(x => x.Id.HasValue).ToArray();
                var updateIds = updates.Select(x => x.Id).ToArray();
                var removes   = person.Addresses?
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
                var adds      = phones.Where(x => !x.Id.HasValue).ToArray();
                var updates   = phones.Where(x => x.Id.HasValue).ToArray();
                var updateIds = updates.Select(x => x.Id).ToArray();
                var removes   = person.Phones?
                    .Where(x => !updateIds.Contains(x.Id))
                    .Select(x => mapper.Map<PhoneData>(x))
                    .ToArray();

                relationships.AddRange(adds.Select(add => new CreatePhone(add)));
                relationships.AddRange(updates.Select(update => new UpdatePhone(update)));

                if (removes != null)
                    relationships.AddRange(removes.Select(remove => new RemovePhone(remove)));
            }

            return Task.FromResult(relationships.ToArray());
        }

        protected override Task<Person> Person(int? id, StashOf<Person> person)
        {
            return person.GetOrPut(async _ =>
                (await _repository.FindAsync(new GetPeopleById(id)
                {
                    IncludeEmails    = true,
                    IncludeAddresses = true,
                    IncludePhones    = true
                })).FirstOrDefault());
        }
    }
}

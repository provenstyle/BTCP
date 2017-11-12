namespace BibleTraining.Api.Person
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Address;
    using Email;
    using Entities;
    using Improving.Highway.Data.Scope.Repository;
    using Miruken;
    using Miruken.Callback;
    using Miruken.Map;
    using Phone;

    public class PersonAggregateHandler : PersonAggregateHandlerBase
    {
        public PersonAggregateHandler(IRepository<IBibleTrainingDomain> repository) : base(repository)
        {
        }

        public override async Task<object[]> GetUpdateRelationships(
            UpdatePerson request,
            Person person,
            IHandler composer)
        {
            var relationships = new List<object>();

            var emails = request.Resource.Emails;
            if (emails != null)
            {
                var adds = emails.Where(x => !x.Id.HasValue).ToArray();
                var updates = emails.Where(x => x.Id.HasValue).ToArray();
                var updateIds = updates.Select(x => x.Id).ToArray();
                var removes = person.Emails?
                    .Where(x => !updateIds.Contains(x.Id))
                    .Select(x => composer.Proxy<IMapping>().Map<EmailData>(x))
                    .ToArray();

                foreach (var add in adds)
                    relationships.Add(new CreateEmail(add));

                foreach (var update in updates)
                    relationships.Add(new UpdateEmail(update));

                foreach (var remove in removes)
                    relationships.Add(new RemoveEmail(remove));
            }

            var addresses = request.Resource.Addresses;
            if (addresses != null)
            {
                var adds = addresses.Where(x => !x.Id.HasValue).ToArray();
                var updates = addresses.Where(x => x.Id.HasValue).ToArray();
                var updateIds = updates.Select(x => x.Id).ToArray();
                var removes = person.Addresses?
                    .Where(x => !updateIds.Contains(x.Id))
                    .Select(x => composer.Proxy<IMapping>().Map<AddressData>(x))
                    .ToArray();

                foreach (var add in adds)
                    relationships.Add(new CreateAddress(add));

                foreach (var update in updates)
                    relationships.Add(new UpdateAddress(update));

                foreach (var remove in removes)
                    relationships.Add(new RemoveAddress(remove));
            }

            var phones = request.Resource.Phones;
            if (phones != null)
            {
                var adds = phones.Where(x => !x.Id.HasValue).ToArray();
                var updates = phones.Where(x => x.Id.HasValue).ToArray();
                var updateIds = updates.Select(x => x.Id).ToArray();
                var removes = person.Phones?
                    .Where(x => !updateIds.Contains(x.Id))
                    .Select(x => composer.Proxy<IMapping>().Map<PhoneData>(x))
                    .ToArray();

                foreach (var add in adds)
                    relationships.Add(new CreatePhone(add));

                foreach (var update in updates)
                    relationships.Add(new UpdatePhone(update));

                foreach (var remove in removes)
                    relationships.Add(new RemovePhone(remove));
            }

            return relationships.ToArray();
        }
    }
}

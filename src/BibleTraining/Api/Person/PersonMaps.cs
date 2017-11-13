namespace BibleTraining.Api.Person
{
    using System.Linq;
    using Address;
    using Email;
    using Entities;
    using Miruken;
    using Miruken.Callback;
    using Miruken.Map;
    using Phone;

    public class PersonMaps : Handler
    {
        [Maps]
        public PersonData MapPerson(Person person, Mapping mapping, IHandler composer)
        {
            var target = mapping.Target as PersonData ?? new PersonData();

            ResourceMapper.Map(target, person);

            target.FirstName = person.FirstName;
            target.LastName  = person.LastName;
            target.Gender    = person.Gender;
            target.BirthDate = person.BirthDate;
            target.Bio       = person.Bio;
            target.Image     = person.Image;

            if (person.Addresses != null)
            {
                target.Addresses = person.Addresses
                    .Select(x => composer.Proxy<IMapping>().Map<AddressData>(x))
                    .ToList();
            }

            if (person.Emails != null)
            {
                target.Emails = person.Emails
                    .Select(x => composer.Proxy<IMapping>().Map<EmailData>(x))
                    .ToList();
            }

            if (person.Phones != null)
            {
                target.Phones = person.Phones
                    .Select(x => composer.Proxy<IMapping>().Map<PhoneData>(x))
                    .ToList();
            }

            return target;
        }

        [Maps]
        public Person MapPersonData(PersonData data, Mapping mapping, IHandler composer)
        {
            var target = mapping.Target as Person ?? new Person();

            EntityMapper.Map(target, data);

            if (data.FirstName!= null)
                target.FirstName = data.FirstName;

            if (data.LastName!= null)
                target.LastName = data.LastName;

            if (data.Gender.HasValue)
                target.Gender = data.Gender.Value;

            if (data.BirthDate.HasValue)
                target.BirthDate = data.BirthDate.Value;

            if (data.Bio!= null)
                target.Bio = data.Bio;

            if (data.Image!= null)
                target.Image = data.Image;

            //if (data.Addresses != null)
            //{
            //    target.Addresses = data.Addresses
            //        .Select(x => composer.Proxy<IMapping>().Map<Address>(x))
            //        .ToArray();
            //}

            //if (data.Emails != null)
            //{
            //    target.Emails = data.Emails
            //        .Select(x => composer.Proxy<IMapping>().Map<Email>(x))
            //        .ToArray();
            //}

            //if (data.Phones != null)
            //{
            //    target.Phones = data.Phones
            //        .Select(x => composer.Proxy<IMapping>().Map<Phone>(x))
            //        .ToArray();
            //}

            return target;
        }
    }

}

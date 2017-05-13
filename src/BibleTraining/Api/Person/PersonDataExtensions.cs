namespace BibleTraining.Api.Person
{
    using System.Linq;
    using Address;
    using Email;
    using Entities;
    using Phone;

    public static class PersonDataExtensions
    {
        public static PersonData Map(this PersonData data, Person person)
        {
            if (person == null) return null;

            ResourceMapper.Map(data, person);

            data.FirstName  = person.FirstName;
            data.LastName   = person.LastName;
            data.Bio        = person.Bio;
            data.BirthDate  = person.BirthDate;
            data.Gender     = person.Gender;
            data.Image      = person.Image;

            data.Emails     = person.Emails?.Select(x => new EmailData().Map(x)).ToArray();
            data.Addresses  = person.Addresses?.Select(x => new AddressData().Map(x)).ToArray();
            data.Phones     = person.Phones?.Select(x => new PhoneData().Map(x)).ToArray();

            return data;
        }
    }
}
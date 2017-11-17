namespace TestInfrastructure
{
    using System;
    using BibleTraining.Api;
    using BibleTraining.Api.Address;
    using BibleTraining.Api.Email;
    using BibleTraining.Api.Person;
    using BibleTraining.Api.Phone;
    using BibleTraining.Entities;
    using Ploeh.AutoFixture;

    public class BibleTrainingFixture
    {
        public Fixture Fixture;

        public BibleTrainingFixture()
        {
            Fixture = new Fixture();

            Fixture.Customize<Entity>(c =>
                c.With(x => x.Created,  DateTime.Now.Subtract(TimeSpan.FromMinutes(1)))
                 .With(x => x.Modified, DateTime.Now.Subtract(TimeSpan.FromMinutes(1)))
                 .Without(x => x.Id));

            Fixture.Customize<Address>(c =>
                c.Without(x => x.AddressTypeId)
                 .Without(x => x.PersonId));

            Fixture.Customize<AddressData>(c =>
                c.Without(x => x.AddressTypeId)
                 .Without(x => x.PersonId));

            Fixture.Customize<Entity>(c =>
                c.Without(x => x.Id)
                 .Without(x => x.RowVersion));

            Fixture.Customize<Resource<int?>>(c =>
                c.Without(x => x.Id)
                 .Without(x => x.RowVersion));

            Fixture.Customize<Email>(c =>
                c.Without(x => x.EmailTypeId)
                 .Without(x => x.PersonId));

            Fixture.Customize<EmailData>(c =>
                c.Without(x => x.EmailTypeId)
                 .Without(x => x.PersonId));

            Fixture.Customize<Person>(c =>
                c.Without(x => x.Addresses)
                 .Without(x => x.Emails)
                 .Without(x => x.Phones));

            Fixture.Customize<PersonData>(c =>
                c.Without(x => x.Addresses)
                 .Without(x => x.Emails)
                 .Without(x => x.Phones));

            Fixture.Customize<Phone>(c =>
                c.Without(x => x.PhoneTypeId)
                 .Without(x => x.PersonId));

            Fixture.Customize<PhoneData>(c =>
                c.Without(x => x.PhoneTypeId)
                 .Without(x => x.PersonId));
        }
    }
}

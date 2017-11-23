namespace TestInfrastructure
{
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

            WithAddress();
            WithEmail();
            WithPhone();

            Fixture.Customize<Entity>(c =>
                c.Without(x => x.Id)
                 .Without(x => x.RowVersion));

            Fixture.Customize<Resource<int?>>(c =>
                c.Without(x => x.Id)
                 .Without(x => x.RowVersion));

            Fixture.Customize<Person>(c =>
                c.Without(x => x.Addresses)
                 .Without(x => x.Emails)
                 .Without(x => x.Phones));

            Fixture.Customize<PersonData>(c =>
                c.Without(x => x.Addresses)
                 .Without(x => x.Emails)
                 .Without(x => x.Phones));
        }

        public BibleTrainingFixture WithAddress(int addressTypeId = 0)
        {
            Fixture.Customize<Address>(c =>
                c.With(x => x.AddressTypeId, addressTypeId)
                 .Without(x => x.PersonId));

            Fixture.Customize<AddressData>(c =>
                c.With(x => x.AddressTypeId, addressTypeId)
                 .Without(x => x.PersonId));
            return this;
        }

        public BibleTrainingFixture WithEmail(int emailTypeId = 0)
        {
            Fixture.Customize<Email>(c =>
                c.With(x => x.EmailTypeId, emailTypeId)
                 .Without(x => x.PersonId)
                 .With(x => x.Address, "a@a.com"));

            Fixture.Customize<EmailData>(c =>
                c.With(x => x.EmailTypeId, emailTypeId)
                 .Without(x => x.PersonId)
                 .With(x => x.Address, "a@a.com"));

            return this;
        }

        public BibleTrainingFixture WithPhone(int phoneTypeId = 0)
        {
            Fixture.Customize<Phone>(c =>
                c.With(x => x.PhoneTypeId, phoneTypeId)
                 .Without(x => x.PersonId)
                 .With(x => x.Number, "1 940 395 5555"));

            Fixture.Customize<PhoneData>(c =>
                c.With(x => x.PhoneTypeId, phoneTypeId)
                 .With(x => x.Number, "1 940 395 5555")
                 .Without(x => x.PersonId));

            return this;
        }
    }
}

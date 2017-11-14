namespace TestInfrastructure
{
    using BibleTraining.Api;
    using BibleTraining.Api.Email;
    using BibleTraining.Api.Person;
    using BibleTraining.Entities;
    using Ploeh.AutoFixture;

    public class BibleTrainingFixture
    {
        public Fixture Fixture;

        public BibleTrainingFixture()
        {
            Fixture = new Fixture();

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
        }
    }
}

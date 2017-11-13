namespace TestInfrastructure
{
    using BibleTraining.Api;
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

            Fixture.Customize<Person>(c =>
                c.Without(x => x.Addresses)
                 .Without(x => x.Emails)
                 .Without(x => x.Phones));
        }
    }
}

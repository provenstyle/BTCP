namespace IntegrationTests.ApiTests
{
    using System.Linq;
    using System.Threading.Tasks;
    using BibleTraining.Api.Email;
    using BibleTraining.Api.Person;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Miruken.Mediate;
    using Scenarios;
    using Ploeh.AutoFixture;

    [TestClass]
    public class PersonAggregateHandlerTests : BibleTrainingScenario
    {
        [TestMethod]
        public async Task CanAddEmail()
        {
            //RollBack(async () =>
            // {
                var personData = Fixture.Create<PersonData>();
                var createResult = await Handler.Send(new CreatePerson(personData));
                var personId = createResult.Id.Value;

                 var person = await GetPerson(personId);

                 person.Emails.Add(Fixture.Create<EmailData>());

                 await Handler.Send(new UpdatePerson(person));

                 person = await GetPerson(personId);
                 Assert.AreEqual(4, person.Emails.Count);
             //});
        }

        private async Task<PersonData> GetPerson(int id)
        {
             return (await Handler.Send(new GetPeople(id)
             {
                 IncludeAddresses = true,
                 IncludeEmails = true,
                 IncludePhones = true
             })).People.FirstOrDefault();
        }
    }
}

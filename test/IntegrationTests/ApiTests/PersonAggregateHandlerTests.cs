namespace IntegrationTests.ApiTests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using BibleTraining.Api.Address;
    using BibleTraining.Api.AddressType;
    using BibleTraining.Api.Email;
    using BibleTraining.Api.EmailType;
    using BibleTraining.Api.Person;
    using BibleTraining.Api.Phone;
    using BibleTraining.Api.PhoneType;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Miruken.Mediate;
    using Scenarios;
    using Ploeh.AutoFixture;

    [TestClass]
    public class PersonAggregateHandlerTests : BibleTrainingScenario
    {
        private async Task<PersonData> GetPerson(int id)
        {
             return (await Handler.Send(new GetPeople(id)
             {
                 IncludeAddresses = true,
                 IncludeEmails    = true,
                 IncludePhones    = true
             })).People.FirstOrDefault();
        }

        public async Task WithCreated(Func<PersonData, Task> testAction)
        {
            await RollBack(async () =>
             {
                 var addressTypeId = (await Handler.Send(new CreateAddressType(Fixture.Create<AddressTypeData>()))).Id;
                 Fixture.Customize<AddressData>(c => c.With(x => x.AddressTypeId, addressTypeId));

                 var emailTypeId = (await Handler.Send(new CreateEmailType(Fixture.Create<EmailTypeData>()))).Id;
                 Fixture.Customize<EmailData>(c => c.With(x => x.EmailTypeId, emailTypeId));

                 var phoneTypeId = (await Handler.Send(new CreatePhoneType(Fixture.Create<PhoneTypeData>()))).Id;
                 Fixture.Customize<PhoneData>(c => c.With(x => x.PhoneTypeId, phoneTypeId));

                 var personData = Fixture.Create<PersonData>();
                 personData.Addresses = Fixture.CreateMany<AddressData>(3).ToList();
                 personData.Emails    = Fixture.CreateMany<EmailData>(3).ToList();
                 personData.Phones    = Fixture.CreateMany<PhoneData>(3).ToList();

                 var createResult = await Handler.Send(new CreatePerson(personData));
                 var created = await GetPerson(createResult.Id ?? -1);
                 await testAction(created);
             });
        }

        [TestMethod]
        public async Task CanCreate()
        {
            await WithCreated(person =>
              {
                  Assert.IsNotNull(person);
                  return Task.FromResult(true);
              });
        }

        [TestMethod]
        public async Task CanUpdate()
        {
            await WithCreated(async created =>
              {
                  const string name = "a";
                  created.FirstName = name;
                  await Handler.Publish(new UpdatePerson(created));
                  var updated = await GetPerson(created.Id ?? 1);
                  Assert.AreEqual(name, updated.FirstName);
              });
        }

        [TestMethod]
        public async Task CreatesRelationshipsOnCreate()
        {
            await WithCreated(person =>
              {
                  Assert.AreEqual(3, person.Addresses.Count);
                  Assert.AreEqual(3, person.Emails.Count);
                  Assert.AreEqual(3, person.Phones.Count);
                  return Task.FromResult(true);
              });
        }

        [TestMethod]
        public async Task CanAddEmail()
        {
            await WithCreated(async created =>
              {
                 var beforeEmailCount = created.Emails.Count;
                 created.Emails.Add(Fixture.Create<EmailData>());
                 await Handler.Send(new UpdatePerson(created));
                 var updated = await GetPerson(created.Id ?? -1);
                 Assert.AreEqual(beforeEmailCount + 1, updated.Emails.Count);
              });
        }

    }
}

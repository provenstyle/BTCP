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
    using FluentValidation;
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
                 Fixture.Customize<EmailData>(c =>
                     c.With(x => x.EmailTypeId, emailTypeId)
                      .With(x => x.Address, "a@a.com")
                      .Without(x => x.PersonId));

                 var phoneTypeId = (await Handler.Send(new CreatePhoneType(Fixture.Create<PhoneTypeData>()))).Id;
                 Fixture.Customize<PhoneData>(c => c.With(x => x.PhoneTypeId, phoneTypeId));

                 var personData = Fixture.Create<PersonData>();
                 personData.Addresses = Fixture.CreateMany<AddressData>(3).ToList();
                 personData.Emails    = Fixture.CreateMany<EmailData>(3).ToList();
                 personData.Phones    = Fixture.CreateMany<PhoneData>(3).ToList();

                 var createResult = await Handler.Send(new CreatePerson(personData));
                 var created = await GetPerson(createResult.Id ?? -1);
                 Assert.IsNotNull(created);

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
        public async Task CanRemove()
        {
            await WithCreated(async created =>
              {
                  await Handler.Publish(new RemovePerson(created));
                  var updated = await GetPerson(created.Id ?? 1);
                  Assert.IsNull(updated);
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
        public async Task CanAddAddress()
        {
            await WithCreated(async created =>
              {
                 var beforeAddressCount = created.Addresses.Count;
                 created.Addresses.Add(Fixture.Create<AddressData>());
                 await Handler.Send(new UpdatePerson(created));
                 var updated = await GetPerson(created.Id ?? -1);
                 Assert.AreEqual(beforeAddressCount + 1, updated.Addresses.Count);
              });
        }

        [TestMethod]
        public async Task CanUpdateAddress()
        {
            await WithCreated(async created =>
              {
                 var address = created.Addresses.Last();
                 address.Name = "a";
                 await Handler.Send(new UpdatePerson(created));
                 var updated = await GetPerson(created.Id ?? -1);
                 Assert.AreEqual(address.Name, updated.Addresses.Last().Name);
              });
        }

        [TestMethod]
        public async Task CanRemoveAddress()
        {
            await WithCreated(async created =>
              {
                  var beforeAddressCount = created.Addresses.Count;
                  var removed = created.Addresses.Last();
                  created.Addresses.Remove(removed);
                  await Handler.Send(new UpdatePerson(created));
                  var updated = await GetPerson(created.Id ?? -1);
                  Assert.AreEqual(beforeAddressCount - 1, updated.Addresses.Count);
                  Assert.IsTrue(updated.Addresses.All(x => x.Id != removed.Id));
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

        [TestMethod]
        public async Task CanUpdateEmail()
        {
            await WithCreated(async created =>
              {
                 var email = created.Emails.Last();
                 email.Address = "a@a.com";
                 await Handler.Send(new UpdatePerson(created));
                 var updated = await GetPerson(created.Id ?? -1);
                 Assert.AreEqual(email.Address, updated.Emails.Last().Address);
              });
        }

        [TestMethod, ExpectedException(typeof(ValidationException))]
        public async Task InvalidEmailAddressUpdateThrows()
        {
            await WithCreated(async created =>
              {
                 var email = created.Emails.Last();
                 email.Address = "a";
                 await Handler.Send(new UpdatePerson(created));
              });
        }

        [TestMethod]
        public async Task CanRemoveEmail()
        {
            await WithCreated(async created =>
              {
                  var beforeEmailCount = created.Emails.Count;
                  var removed = created.Emails.Last();
                  created.Emails.Remove(removed);
                  await Handler.Send(new UpdatePerson(created));
                  var updated = await GetPerson(created.Id ?? -1);
                  Assert.AreEqual(beforeEmailCount - 1, updated.Emails.Count);
                  Assert.IsTrue(updated.Emails.All(x => x.Id != removed.Id));
              });
        }

        [TestMethod]
        public async Task CanAddPhone()
        {
            await WithCreated(async created =>
              {
                 var beforePhoneCount = created.Phones.Count;
                 created.Phones.Add(Fixture.Create<PhoneData>());
                 await Handler.Send(new UpdatePerson(created));
                 var updated = await GetPerson(created.Id ?? -1);
                 Assert.AreEqual(beforePhoneCount + 1, updated.Phones.Count);
              });
        }

        [TestMethod]
        public async Task CanUpdatePhone()
        {
            await WithCreated(async created =>
              {
                 var phone = created.Phones.Last();
                 phone.Name = "a";
                 await Handler.Send(new UpdatePerson(created));
                 var updated = await GetPerson(created.Id ?? -1);
                 Assert.AreEqual(phone.Name, updated.Phones.Last().Name);
              });
        }

        [TestMethod]
        public async Task CanRemovePhone()
        {
            await WithCreated(async created =>
              {
                  var beforePhoneCount = created.Phones.Count;
                  var removed = created.Phones.Last();
                  created.Phones.Remove(removed);
                  await Handler.Send(new UpdatePerson(created));
                  var updated = await GetPerson(created.Id ?? -1);
                  Assert.AreEqual(beforePhoneCount - 1, updated.Phones.Count);
                  Assert.IsTrue(updated.Phones.All(x => x.Id != removed.Id));
              });
        }

    }
}

namespace IntegrationTests
{
    using System.Linq;
    using BibleTraining.Entities;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Scenarios;

    [TestClass]
    public class PopulatingRelationshipsInEntityFramework : BibleTrainingScenario
    {
        private Email email;
        private EmailType emailType;
        private Person person;

        [TestInitialize]
        public override void TestInitialize()
        {
            base.TestInitialize();

            email = new Email()
            {
                Address = "a@a.com",
                CreatedBy = "a",
                ModifiedBy = "a",
            };
            emailType = new EmailType
            {
                Name = "a",
                CreatedBy = "a",
                ModifiedBy = "a"
            };
            person = new Person
            {
                FirstName = "a",
                LastName = "a",
                CreatedBy = "a",
                ModifiedBy = "a"
            };
        }

        [TestMethod]
        public void WhenNeitherObjectExists_ThroughNavigationProperties()
        {
            RollBack(() =>
             {
                 using (var scope = Repository.Scopes.Create())
                 {
                     email.EmailType = emailType;
                     email.Person = person;

                     Repository.Context.Add(email);
                     scope.SaveChanges();

                     Assert.IsTrue(email.Id > 0);
                     Assert.IsTrue(email.EmailType.Id > 0);
                     Assert.IsTrue(email.Person.Id > 0);

                     Assert.AreEqual(email.PersonId, email.Person.Id);
                     Assert.AreEqual(email.EmailTypeId, email.EmailType.Id);
                 }
             });
        }

        [TestMethod]
        public void WhenObjectsAlreadyExists_ThroughIds()
        {
            RollBack(() =>
             {
                 using (var scope = Repository.Scopes.Create())
                 {
                     Repository.Context.Add(emailType);
                     Repository.Context.Add(person);
                     scope.SaveChanges();

                     Assert.IsTrue(emailType.Id > 0);
                     Assert.IsTrue(person.Id > 0);
                 }

                 using (var scope = Repository.Scopes.Create())
                 {
                     email.EmailTypeId = emailType.Id;
                     email.PersonId = person.Id;

                     Repository.Context.Add(email);
                     scope.SaveChanges();

                     Assert.IsTrue(email.Id > 0);

                     Assert.AreEqual(email.PersonId, person.Id);
                     Assert.AreEqual(email.EmailTypeId, emailType.Id);
                 }
             });
        }

        [TestMethod]
        public void WhenObjectsAlreadyExists_InTheSameContext_ThroughNavigationProperties()
        {
            RollBack(() =>
             {
                 using (var scope = Repository.Scopes.Create())
                 {
                     Repository.Context.Add(emailType);
                     Repository.Context.Add(person);
                     scope.SaveChanges();

                     Assert.IsTrue(emailType.Id > 0);
                     Assert.IsTrue(person.Id > 0);
                 }

                 using (var scope = Repository.Scopes.Create())
                 {
                     var existingEmailType = Context.AsQueryable<EmailType>()
                         .FirstOrDefault(x => x.Id == emailType.Id);
                     var existingPerson = Context.AsQueryable<Person>()
                         .FirstOrDefault(x => x.Id == person.Id);
                     var beforeEmailTypeCount = Context.AsQueryable<EmailType>().Count();
                     var beforePersonCount = Context.AsQueryable<Person>().Count();

                     email.EmailType = existingEmailType;
                     email.Person = existingPerson;

                     Repository.Context.Add(email);
                     scope.SaveChanges();

                     Assert.IsTrue(email.Id > 0);

                     Assert.AreEqual(email.PersonId,    existingPerson?.Id);
                     Assert.AreEqual(email.EmailTypeId, existingEmailType?.Id);

                     Assert.AreEqual(beforeEmailTypeCount, Context.AsQueryable<EmailType>().Count());
                     Assert.AreEqual(beforePersonCount, Context.AsQueryable<Person>().Count());
                 }
             });
        }

        ///This scenario causes unintended results
        ///What we wanted to do was create a relationship between
        ///the new entity and existing entities
        ///In actuallity we created all new objects
        [TestMethod]
        public void WhenObjectsAlreadyExists_FromDifferentContexts_ThroughNavigationProperties_CausesUnintendedResults()
        {
            RollBack(() =>
             {
                 using (var scope = Repository.Scopes.Create())
                 {
                     Repository.Context.Add(emailType);
                     Repository.Context.Add(person);
                     scope.SaveChanges();

                     Assert.IsTrue(emailType.Id > 0);
                     Assert.IsTrue(person.Id > 0);
                 }

                 using (var scope = Repository.Scopes.Create())
                 {
                     var beforeEmailTypeCount = Context.AsQueryable<EmailType>().Count();
                     var beforePersonCount = Context.AsQueryable<Person>().Count();

                     email.EmailType = emailType;
                     email.Person = person;

                     Repository.Context.Add(email);
                     scope.SaveChanges();

                     Assert.IsTrue(email.Id > 0);

                     Assert.AreEqual(email.PersonId, person.Id);
                     Assert.AreEqual(email.EmailTypeId, emailType.Id);

                     //Here is the surprise
                     Assert.AreEqual(beforeEmailTypeCount + 1, Context.AsQueryable<EmailType>().Count());
                     Assert.AreEqual(beforePersonCount + 1, Context.AsQueryable<Person>().Count());
                 }
             });
        }
    }
}

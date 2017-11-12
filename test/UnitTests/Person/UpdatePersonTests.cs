namespace UnitTests.Person
{
    using System.Linq;
    using System.Threading.Tasks;
    using BibleTraining.Api.Person;
    using BibleTraining.Entities;
    using FizzWare.NBuilder;
    using Infrastructure;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Miruken.Mediate;
    using Rhino.Mocks;
    using UnitTests;

    [TestClass]
    public class UpdatePersonTests : TestScenario
    {
        [TestMethod]
        public async Task ShouldUpdatePerson()
        {
            var person= new Person()
            {
                Id         = 1,
                FirstName  = "A",
                LastName   = "B",
                RowVersion = new byte[] { 0x01 }
            };

            var personData = Builder<PersonData>.CreateNew()
                .With(c => c.Id = 1).And(c => c.RowVersion = new byte[] { 0x01 })
                .Build();

            _context.Expect(c => c.AsQueryable<Person>())
                .Return(new[] { person }.AsQueryable().TestAsync());

            _context.Expect(c => c.CommitAsync())
                .WhenCalled(inv => person.RowVersion = new byte[] { 0x02 })
                .Return(Task.FromResult(1));

            var result = await _handler.Send(new UpdatePerson(personData));
            Assert.AreEqual(1, result.Id);
            CollectionAssert.AreEqual(new byte[] { 0x02 }, result.RowVersion);

            Assert.AreEqual(personData.FirstName, person.FirstName);
            Assert.AreEqual(personData.LastName, person.LastName);
            Assert.AreEqual(personData.Bio, person.Bio);
            Assert.AreEqual(personData.BirthDate, person.BirthDate);
            Assert.AreEqual(personData.Image, person.Image);

            _context.VerifyAllExpectations();
        }
    }
}
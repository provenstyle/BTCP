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
    public class RemovePersonTests : TestScenario
    {
        [TestMethod]
        public async Task ShouldRemovePerson()
        {
            var entity = new Person
            {
                Id = 1,
                FirstName = "A",
                LastName = "B",
                RowVersion = new byte[] { 0x01 }
            };

            var personData = Builder<PersonData>.CreateNew()
                .With(pg => pg.Id = 1).And(c => c.RowVersion = new byte[] { 0x01 })
                .Build();

            _context.Expect(pg => pg.AsQueryable<Person>())
                .Return(new[] { entity }.AsQueryable().TestAsync());

            _context.Expect(c => c.Remove(entity))
                .Return(entity);

            _context.Expect(c => c.CommitAsync())
                .Return(Task.FromResult(1));

            var result = await _handler.Send(new RemovePerson(personData));
            Assert.AreEqual(1, result.Id);
            CollectionAssert.AreEqual(new byte[] { 0x01 }, result.RowVersion);

            _context.VerifyAllExpectations();
        }
    }
}
namespace UnitTests.Person
{
    using System.Threading.Tasks;
    using BibleTraining.Api.Person;
    using BibleTraining.Entities;
    using FizzWare.NBuilder;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Miruken.Mediate;
    using Rhino.Mocks;
    using UnitTests;

    [TestClass]
    public class CreatePersonTests : TestScenario
    {
        [TestMethod]
        public async Task ShouldCreatePerson()
        {
            var person = Builder<PersonData>.CreateNew()
                .With(pg => pg.Id = 0).And(pg => pg.RowVersion = null)
                .Build();

            _context.Expect(pg => pg.Add(Arg<Person>.Is.Anything))
                .WhenCalled(inv =>
                                {
                                    var entity = (Person)inv.Arguments[0];
                                    entity.Id         = 1;
                                    entity.RowVersion = new byte[] { 0x01 };
                                    Assert.AreEqual(person.FirstName, entity.FirstName);
                                    inv.ReturnValue = entity;
                                }).Return(null);

            _context.Expect(pg => pg.CommitAsync())
                .Return(Task.FromResult(1));

            var result = await _handler.Send(new CreatePerson(person));
            Assert.AreEqual(1, result.Id);
            CollectionAssert.AreEqual(new byte[] { 0x01 }, result.RowVersion);

            _context.VerifyAllExpectations();
        }
    }
}
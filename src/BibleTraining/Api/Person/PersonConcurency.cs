namespace BibleTraining.Api.Person
{
    using Concurrency;
    using Entities;

    public class PersonConcurency : CheckConcurrency<Person, PersonData>
    {
    }
}

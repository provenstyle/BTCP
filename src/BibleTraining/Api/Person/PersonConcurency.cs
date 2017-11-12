namespace BibleTraining.Api.Person
{
    using Entities;
    using Concurrency;

    public class PersonConcurency : CheckConcurrency<Person, PersonData>
    {
    }
}

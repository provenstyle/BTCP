namespace BibleTraining.Api.Person
{
    using Entities;
    using Improving.Highway.Data.Scope.Concurrency;
    using Improving.MediatR;

    [RelativeOrder(5), StopOnFailure]
    public class PersonConcurency : CheckConcurrency<Person, PersonData>
    {
    }
}
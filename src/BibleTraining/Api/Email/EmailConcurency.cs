namespace BibleTraining.Api.Email
{
    using Entities;
    using Improving.Highway.Data.Scope.Concurrency;
    using Improving.MediatR;

    [RelativeOrder(5), StopOnFailure]
    public class EmailConcurency : CheckConcurrency<Email, EmailData>
    {
    }
}
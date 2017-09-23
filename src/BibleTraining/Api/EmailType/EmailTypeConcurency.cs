namespace BibleTraining.Api.EmailType
{
    using Entities;
    using Improving.Highway.Data.Scope.Concurrency;

    [RelativeOrder(5), StopOnFailure]
    public class EmailTypeConcurency : CheckConcurrency<EmailType, EmailTypeData>
    {
    }
}
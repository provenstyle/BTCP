namespace BibleTraining.Api.ContactType
{
    using Entities;
    using Improving.Highway.Data.Scope.Concurrency;
    using Improving.MediatR;

    [RelativeOrder(5), StopOnFailure]
    public class ContactTypeConcurency : CheckConcurrency<EmailType, ContactTypeData>
    {
    }
}
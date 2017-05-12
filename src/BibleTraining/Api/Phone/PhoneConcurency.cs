namespace BibleTraining.Api.Phone
{
    using Entities;
    using Improving.Highway.Data.Scope.Concurrency;
    using Improving.MediatR;

    [RelativeOrder(5), StopOnFailure]
    public class PhoneConcurency : CheckConcurrency<Phone, PhoneData>
    {
    }
}
namespace BibleTraining.Api.Phone
{
    using Entities;
    using Improving.Highway.Data.Scope.Concurrency;

    [RelativeOrder(5), StopOnFailure]
    public class PhoneConcurency : CheckConcurrency<Phone, PhoneData>
    {
    }
}
namespace BibleTraining.Api.PhoneType
{
    using Improving.MediatR;
    using Improving.Highway.Data.Scope.Concurrency;
    using Entities;

    [RelativeOrder(5), StopOnFailure]
    public class PhoneTypeConcurency : CheckConcurrency<PhoneType, PhoneTypeData>
    {
    }
}

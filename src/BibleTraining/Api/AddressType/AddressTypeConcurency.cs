namespace BibleTraining.Api.AddressType
{
    using Improving.Highway.Data.Scope.Concurrency;
    using Improving.MediatR;

    [RelativeOrder(5), StopOnFailure]
    public class AddressTypeConcurency : CheckConcurrency<AddressType, AddressTypeData>
    {
    }
}
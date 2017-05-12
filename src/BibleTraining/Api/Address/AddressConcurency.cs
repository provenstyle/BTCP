namespace BibleTraining.Api.Address
{
    using Entities;
    using Improving.Highway.Data.Scope.Concurrency;
    using Improving.MediatR;

    [RelativeOrder(5), StopOnFailure]
    public class AddressConcurency : CheckConcurrency<Address, AddressData>
    {
    }
}
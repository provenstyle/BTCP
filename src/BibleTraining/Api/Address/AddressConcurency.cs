namespace BibleTraining.Api.Address
{
    using Concurrency;
    using Entities;

    //[RelativeOrder(5), StopOnFailure]
    public class AddressConcurency : CheckConcurrency<Address, AddressData>
    {
    }
}
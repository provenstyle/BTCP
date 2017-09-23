namespace BibleTraining.Api.Address
{
    using Entities;

    //[RelativeOrder(5), StopOnFailure]
    public class AddressConcurency : CheckConcurrency<Address, AddressData>
    {
    }
}
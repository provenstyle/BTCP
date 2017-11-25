namespace BibleTraining.Api.Address
{
    using Concurrency;
    using Entities;

    public class AddressConcurency : CheckConcurrency<Address, AddressData>
    {
    }
}
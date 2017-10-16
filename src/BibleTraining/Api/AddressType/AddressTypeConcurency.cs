namespace BibleTraining.Api.AddressType
{
    using Concurrency;
    using Entities;

    public class AddressTypeConcurency : CheckConcurrency<AddressType, AddressTypeData>
    {
    }
}

namespace BibleTraining.Api.AddressType
{
    using Entities;

    public class AddressTypeConcurency : CheckConcurrency<AddressType, AddressTypeData>
    {
    }
}

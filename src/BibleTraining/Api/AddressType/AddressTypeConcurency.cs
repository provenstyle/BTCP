namespace BibleTraining.Api.AddressType
{
    using Entities;

    //[RelativeOrder(5), StopOnFailure]
    public class AddressTypeConcurency : CheckConcurrency<AddressType, AddressTypeData>
    {
    }
}
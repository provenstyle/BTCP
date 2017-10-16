namespace BibleTraining.Api.PhoneType
{
    using Concurrency;
    using Entities;

    public class PhoneTypeConcurency : CheckConcurrency<PhoneType, PhoneTypeData>
    {
    }
}

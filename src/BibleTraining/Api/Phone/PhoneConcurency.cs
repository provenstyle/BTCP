namespace BibleTraining.Api.Phone
{
    using Concurrency;
    using Entities;

    public class PhoneConcurency : CheckConcurrency<Phone, PhoneData>
    {
    }
}

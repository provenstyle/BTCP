namespace BibleTraining.Api.EmailType
{
    using Concurrency;
    using Entities;

    public class EmailTypeConcurency : CheckConcurrency<EmailType, EmailTypeData>
    {
    }
}

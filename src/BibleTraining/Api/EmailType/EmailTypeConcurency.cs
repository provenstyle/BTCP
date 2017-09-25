namespace BibleTraining.Api.EmailType
{
    using Entities;

    public class EmailTypeConcurency : CheckConcurrency<EmailType, EmailTypeData>
    {
    }
}

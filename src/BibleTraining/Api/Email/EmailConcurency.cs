namespace BibleTraining.Api.Email
{
    using Concurrency;
    using Entities;

    public class EmailConcurency : CheckConcurrency<Email, EmailData>
    {
    }
}
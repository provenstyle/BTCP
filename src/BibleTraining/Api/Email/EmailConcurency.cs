namespace BibleTraining.Api.Email
{
    using Entities;

    public class EmailConcurency : CheckConcurrency<Email, EmailData>
    {
    }
}
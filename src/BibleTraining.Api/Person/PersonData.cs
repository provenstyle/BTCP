namespace BibleTraining.Api.Person
{
    using System;
    using Email;
    using Improving.MediatR;

    public class PersonData : Resource<int>
    {
        public string    FirstName { get; set; }
        public string    LastName  { get; set; }
        public Gender?   Gender    { get; set; }
        public DateTime? BirthDate { get; set; }
        public string    Bio       { get; set; }
        public string    Image     { get; set; }

        public EmailData[] Emails  { get; set; }
    }
}
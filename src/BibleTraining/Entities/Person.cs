namespace BibleTraining.Entities
{
    using System;
    using System.Collections.Generic;
    using Api.Person;

    public class Person : Entity
    {
        public string   FirstName { get; set; }
        public string   LastName  { get; set; }
        public Gender   Gender    { get; set; }
        public DateTime BirthDate { get; set; }
        public string   Bio       { get; set; }
        public string   Image     { get; set; }

        public virtual ICollection<Email>   Emails    { get; set; }
        public virtual ICollection<Address> Addresses { get; set; }
    }
}
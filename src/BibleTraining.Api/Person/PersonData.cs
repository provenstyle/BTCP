namespace BibleTraining.Api.Person
{
    using System;
    using Address;
    using Email;
    using Phone;

    public class PersonData : Resource<int?>
    {
        public string    FirstName { get; set; }
        public string    LastName  { get; set; }
        public Gender?   Gender    { get; set; }
        public DateTime? BirthDate { get; set; }
        public string    Bio       { get; set; }
        public string    Image     { get; set; }

        public EmailData[]   Emails    { get; set; }
        public AddressData[] Addresses { get; set; }
        public PhoneData[]   Phones    { get; set; }
    }
}
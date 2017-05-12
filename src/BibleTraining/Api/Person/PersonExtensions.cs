namespace BibleTraining.Api.Person
{
    using System;
    using Entities;

    public static class PersonExtensions
    {
        public static Person Map(this Person person, PersonData data)
        {
            if (data.FirstName != null)
                person.FirstName = data.FirstName;

            if (data.LastName != null)
                person.LastName = data.LastName;

            if (data.Bio != null)
                person.Bio = data.Bio;

            if (data.BirthDate.HasValue)
                person.BirthDate = data.BirthDate.Value;

            if (data.Gender.HasValue)
                person.Gender = data.Gender.Value;

            if (data.Image != null)
                person.Image = data.Image;

            if (data.CreatedBy != null)
                person.CreatedBy = data.CreatedBy;

            if (data.ModifiedBy != null)
                person.ModifiedBy = data.ModifiedBy;

            person.Modified = DateTime.Now;

            return person;
        }
    }
}
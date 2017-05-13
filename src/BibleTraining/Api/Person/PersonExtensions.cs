namespace BibleTraining.Api.Person
{
    using Entities;

    public static class PersonExtensions
    {
        public static Person Map(this Person person, PersonData data)
        {
            if (data == null) return null;

            EntityMapper.Map(person, data);

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

            return person;
        }
    }
}
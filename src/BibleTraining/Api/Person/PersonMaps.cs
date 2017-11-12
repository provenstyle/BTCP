namespace BibleTraining.Api.Person
{
    using Entities;
    using Miruken.Callback;
    using Miruken.Map;

    public class PersonMaps : Handler
    {
        [Maps]
        public PersonData MapPerson(Person person, Mapping mapping)
        {
            var target = mapping.Target as PersonData ?? new PersonData();

            ResourceMapper.Map(target, person);

            target.FirstName = person.FirstName;
            target.LastName  = person.LastName;
            target.Gender    = person.Gender;
            target.BirthDate = person.BirthDate;
            target.Bio       = person.Bio;
            target.Image     = person.Image;

            return target;
        }

        [Maps]
        public Person MapPersonData(PersonData data, Mapping mapping)
        {
            var target = mapping.Target as Person ?? new Person();

            EntityMapper.Map(target, data);

            if (data.FirstName!= null)
                target.FirstName = data.FirstName;

            if (data.LastName!= null)
                target.LastName = data.LastName;

            if (data.Gender.HasValue)
                target.Gender = data.Gender.Value;

            if (data.BirthDate.HasValue)
                target.BirthDate = data.BirthDate.Value;

            if (data.Bio!= null)
                target.Bio = data.Bio;

            if (data.Image!= null)
                target.Image = data.Image;

            return target;
        }
    }

}

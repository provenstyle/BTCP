namespace BibleTraining.Api.Person
{
    public class UpdatePerson : UpdateResource<PersonData, int?>
    {
        public UpdatePerson()
        {
        }

        public UpdatePerson(PersonData person)
            : base(person)
        {
        }
    }
}
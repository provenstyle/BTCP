namespace BibleTraining.Api.Person
{
    public class RemovePerson : UpdateResource<PersonData, int?>
    {
        public RemovePerson()
        {
        }

        public RemovePerson(PersonData person)
            : base(person)
        {
        }
    }
}
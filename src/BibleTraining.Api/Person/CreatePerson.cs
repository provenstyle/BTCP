namespace BibleTraining.Api.Person
{
    public class CreatePerson : 
        ResourceAction<PersonData, int?>,
        IValidateCreateUpdatePerson
    {
        public CreatePerson()
        {
        }

        public CreatePerson(PersonData person)
            : base (person)
        {
        }
    }
}
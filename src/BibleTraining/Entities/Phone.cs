namespace BibleTraining.Entities
{
    using Improving.MediatR;

    public class Phone : Entity, IKeyProperties<int>
    {
        public string Name        { get; set; }
        public string Description { get; set; }

        public int    PersonId    { get; set; }
    }
}
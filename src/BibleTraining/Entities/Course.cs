namespace BibleTraining.Entities
{
    using Improving.MediatR;

    public class Course : Entity, IKeyProperties<int>
    {
        public string Name        { get; set; }
        public string Description { get; set; }
    }
}
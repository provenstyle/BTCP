namespace BibleTraining.Entities
{
    using Api;

    public class Course : Entity, IKeyProperties<int>
    {
        public string Name        { get; set; }
        public string Description { get; set; }
    }
}
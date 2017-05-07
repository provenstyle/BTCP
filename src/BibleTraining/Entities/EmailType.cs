namespace BibleTraining.Entities
{
    using Improving.MediatR;

    public class EmailType : Entity, IKeyProperties<int>
    {
        public string Name { get; set; }
    }
}
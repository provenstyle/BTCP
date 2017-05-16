namespace BibleTraining.Entities
{
    using Improving.MediatR;

    public class PhoneType : Entity, IKeyProperties<int>
    {
        public string Name        { get; set; }
        public string Description { get; set; }
    }
}

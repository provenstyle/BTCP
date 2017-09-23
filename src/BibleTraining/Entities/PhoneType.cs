namespace BibleTraining.Entities
{
    using Api;

    public class PhoneType : Entity, IKeyProperties<int>
    {
        public string Name        { get; set; }
        public string Description { get; set; }
    }
}

namespace BibleTraining.Entities
{
    using Api;

    public class EmailType : Entity, IKeyProperties<int>
    {
        public string Name { get; set; }
    }
}

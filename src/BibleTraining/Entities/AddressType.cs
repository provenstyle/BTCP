namespace BibleTraining.Entities
{
    using Api;

    public class AddressType : Entity, IKeyProperties<int>
    {
        public string Name { get; set; }
    }
}

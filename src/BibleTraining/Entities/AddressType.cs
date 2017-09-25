namespace BibleTraining.Entities
{
	using Api;

    public partial class AddressType : Entity, IKeyProperties<int>
    {
        public string Name { get; set; }
    }
}

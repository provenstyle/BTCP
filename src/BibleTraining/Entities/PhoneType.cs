namespace BibleTraining.Entities
{
	using Api;

    public partial class PhoneType : Entity, IKeyProperties<int>
    {
        public string Name { get; set; }
    }
}

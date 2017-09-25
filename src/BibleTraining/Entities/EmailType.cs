namespace BibleTraining.Entities
{
	using Api;

    public partial class EmailType : Entity, IKeyProperties<int>
    {
        public string Name { get; set; }
    }
}

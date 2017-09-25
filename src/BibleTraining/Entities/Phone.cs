namespace BibleTraining.Entities
{
	using Api;

    public partial class Phone : Entity, IKeyProperties<int>
    {
        public string Name { get; set; }
    }
}

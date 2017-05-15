namespace $ApplicationName$.Entities
{
    using Improving.MediatR;

    public class $Entity$ : Entity, IKeyProperties<int>
    {
        public string Name        { get; set; }
        public string Description { get; set; }
    }
}

namespace $ApplicationName$.Api.$Entity$
{
    using Improving.MediatR;

    public class $Entity$Data : Resource<int?>
    {
        public string Name        { get; set; }
        public string Description { get; set; }
    }
}
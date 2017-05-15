namespace $ApplicationName$.Api.$Entity$
{
    using Improving.MediatR;

    public class Get$EntityPlural$ : Request.WithResponse<$Entity$Result>
    {
        public Get$EntityPlural$()
        {
            Ids = new int[0];
        }

        public Get$EntityPlural$(params int[] ids)
        {
            Ids = ids;
        }

        public int[] Ids { get; set;}

        public bool KeyProperties { get; set; }
    }
}

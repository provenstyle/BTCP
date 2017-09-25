namespace $ApplicationName$.Api.$Entity$
{
    using Miruken.Mediate; 
    
    public class Get$EntityPlural$ : IRequest<$Entity$Result>
    {
        public Get$EntityPlural$()
        {
            Ids = new int[0];
        }

        public Get$EntityPlural$(params int[] ids)
        {
            Ids = ids;
        }

        public int[] Ids { get; set; }

        public bool KeyProperties { get; set; }
    }
}

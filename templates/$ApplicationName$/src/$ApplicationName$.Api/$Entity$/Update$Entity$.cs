namespace $ApplicationName$.Api.$Entity$
{
    using Improving.MediatR;

    public class Update$Entity$ : UpdateResource<$Entity$Data, int?>
    {
        public Update$Entity$()
        {
        }

        public Update$Entity$($Entity$Data $entityLowercase$)
            : base($entityLowercase$)
        {          
        }
    }
}

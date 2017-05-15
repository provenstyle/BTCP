namespace $ApplicationName$.Api.$Entity$ 
{
    using Improving.MediatR;

    public class Remove$Entity$ : UpdateResource<$Entity$Data, int?>
    {
        public Remove$Entity$()
        {
        }

        public Remove$Entity$($Entity$Data $entityLowercase$)
            : base($entityLowercase$)
        {
        }
    }
}

namespace $ApplicationName$.Api.$Entity$
{
    using Improving.MediatR;

    public class Create$Entity$ : ResourceAction<$Entity$Data, int?>
    {
        public Create$Entity$()
        {
        }

        public Create$Entity$($Entity$Data $entityLowercase$)
            : base ($entityLowercase$)
        {
        }
    }
}

namespace $ApplicationName$.Api.$Entity$
{

    public static class $Entity$DataExtensions
    {
        public static $Entity$Data Map(this $Entity$Data data, $Entity$ $entityLowercase$)
        {
            if ($entityLowercase$ == null) return null;
            
            ResourceMapper.Map(data, $entityLowercase$);

            data.Name        = $entityLowercase$.Name;
            data.Description = $entityLowercase$.Description;

            return data;
        }
    }
}

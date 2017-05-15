namespace $ApplicationName$.Api.$Entity$
{
    using Entity;

    public static class $Entity$Extensions
    {
        public static $Entity$ Map(this $Entity$ $entityLowercase$, $Entity$Data data)
        {
            if (data == null) return null;

            EntityMapper.Map($entityLowercase$, data);

            if (data.Name != null)
                $entityLowercase$.Name = data.Name;
                
            if (data.Description != null)
                $entityLowercase$.Description = data.Description;

            return $entityLowercase$;
        }
    }
}

namespace $ApplicationName$.Api.$Entity$
{
    using Entities;
    using Miruken.Callback;
    using Miruken.Map;

    public class $Entity$Maps : Handler
    {
        [Maps]
        public $Entity$Data Map$Entity$($Entity$ $entityLowercase$, Mapping mapping)
        {
            var target = mapping.Target as $Entity$Data ?? new $Entity$Data();

            ResourceMapper.Map(target, $entityLowercase$);

            target.Name        = $entityLowercase$.Name;

            return target;
        }

        [Maps]
        public $Entity$ Map$Entity$Data($Entity$Data data, Mapping mapping)
        {
            var target = mapping.Target as $Entity$ ?? new $Entity$();

            EntityMapper.Map(target, data);

            if (data.Name != null)
                target.Name = data.Name;

            return target;
        }
    }

}

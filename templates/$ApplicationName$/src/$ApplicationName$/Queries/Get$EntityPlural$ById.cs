namespace $ApplicationName$.Queries
{
    using System.Linq;
    using Highway.Data;
    using Entities;
    
    public class Get$EntityPlural$ById : Query<$Entity$>
    {
        public bool KeyProperties { get; set; }

        public Get$EntityPlural$ById(int? id)
            :this(new []{ id ?? 0 })
        {
        }


        public Get$EntityPlural$ById(int[] ids)
        {
            ContextQuery = c =>
            {
                var query = Context.AsQueryable<$Entity$>();

                if (ids?.Length == 1)
                {
                    var id = ids[0];
                    query = query.Where(x => x.Id == id);
                }
                else if (ids?.Length > 1)
                {
                    query = query.Where(x => ids.Contains(x.Id));
                }
                
                if (KeyProperties)
                {
                    return query.Select(x => new $Entity${Id = x.Id, Name = x.Name});
                }

                return query;
            };
        }
    }
}

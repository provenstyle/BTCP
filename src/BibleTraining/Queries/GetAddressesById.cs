namespace BibleTraining.Queries
{
    using System.Linq;
    using Entities;
    using Highway.Data;

    public class GetAddressesById : Query<Address>
    {
        public bool KeyProperties { get; set; }

        public GetAddressesById(int? id)
            :this(new [] { id ?? 0 })
        {
        }

        public GetAddressesById(int[] ids)
        {
            ContextQuery = c =>
               {
                   var query = Context.AsQueryable<Address>();

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
                       query = query.Select(x => new Address{Id = x.Id, Name = x.Name});

                   return query;
               };
        }
    }
}
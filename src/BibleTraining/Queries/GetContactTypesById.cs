namespace BibleTraining.Queries
{
    using System.Data.Entity;
    using System.Linq;
    using Entities;
    using Highway.Data;

    public class GetContactTypesById : Query<EmailType>
    {
        public bool KeyProperties { get; set; }

        public GetContactTypesById(int[] ids)
        {
            ContextQuery = c =>
               {
                   var query = Context.AsQueryable<EmailType>().AsNoTracking();

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
                       return query.Select(x => new EmailType { Id = x.Id, Name = x.Name });
                   }

                   return query;
               };
        }
    }
}
namespace BibleTraining.Queries
{
    using System.Linq;
    using Entities;
    using Highway.Data;

    public class GetEmailsById : Query<Email>
    {
        public GetEmailsById(int? id)
            :this(new []{ id ?? 0 })
        {
        }

        public GetEmailsById(int[] ids)
        {
            ContextQuery = c =>
               {
                   var query = Context.AsQueryable<Email>();

                   if (ids?.Length == 1)
                   {
                       var id = ids[0];
                       query = query.Where(x => x.Id == id);
                   }
                   else if (ids?.Length > 1)
                   {
                       query = query.Where(x => ids.Contains(x.Id));
                   }

                   return query;
               };
        }
    }
}
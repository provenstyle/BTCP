namespace BibleTraining.Queries
{
    using System.Data.Entity;
    using System.Linq;
    using Entities;
    using Highway.Data;

    public class GetEmailsById : Query<Email>
    {
        public GetEmailsById(int[] ids)
        {
            ContextQuery = c =>
                               {
                                   var query = Context.AsQueryable<Email>().AsNoTracking();

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
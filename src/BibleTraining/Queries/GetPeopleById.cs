namespace BibleTraining.Queries
{
    using System.Data.Entity;
    using System.Linq;
    using Entities;
    using Highway.Data;

    public class GetPeopleById : Query<Person>
    {
        public GetPeopleById(int[] ids)
        {
            ContextQuery = c =>
               {
                   var query = Context.AsQueryable<Person>().AsNoTracking();

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
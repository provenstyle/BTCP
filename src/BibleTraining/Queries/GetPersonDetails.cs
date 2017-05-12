namespace BibleTraining.Queries
{
    using System.Data.Entity;
    using System.Linq;
    using Entities;
    using Highway.Data;

    public class GetPersonDetails : Scalar<Person>
    {
        public GetPersonDetails(int id)
        {
            ContextQuery = c =>
               {
                   var query = Context.AsQueryable<Person>()
                       .AsNoTracking()
                       .Include(x => x.Emails)
                       .FirstOrDefault(x => x.Id == id);

                   return query;
               };
        }
    }
}
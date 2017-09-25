namespace BibleTraining.Queries
{
    using System.Linq;
    using Highway.Data;
    using Entities;
    
    public class GetPhonesById : Query<Phone>
    {
        public bool KeyProperties { get; set; }

        public GetPhonesById(int? id)
            :this(new []{ id ?? 0 })
        {
        }


        public GetPhonesById(int[] ids)
        {
            ContextQuery = c =>
            {
                var query = Context.AsQueryable<Phone>();

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
                    return query.Select(x => new Phone{Id = x.Id, Name = x.Name});
                }

                return query;
            };
        }
    }
}

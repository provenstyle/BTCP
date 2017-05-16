namespace BibleTraining.Queries
{
    using System.Linq;
    using Highway.Data;
    using Entities;
    
    public class GetPhoneTypesById : Query<PhoneType>
    {
        public bool KeyProperties { get; set; }

        public GetPhoneTypesById(int? id)
            :this(new []{ id ?? 0 })
        {
        }


        public GetPhoneTypesById(int[] ids)
        {
            ContextQuery = c =>
            {
                var query = Context.AsQueryable<PhoneType>();

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
                    return query.Select(x => new PhoneType{Id = x.Id, Name = x.Name});
                }

                return query;
            };
        }
    }
}

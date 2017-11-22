namespace BibleTraining.Api
{
    using Entities;

    public class EntityMapper
    {
        public static Entity Map(Entity entity, Resource<int?> resource)
        {
            if (resource.CreatedBy != null)
                entity.CreatedBy = resource.CreatedBy;

            if (resource.ModifiedBy != null)
                entity.ModifiedBy = resource.ModifiedBy;

            return entity;
        }
    }
}
namespace BibleTraining.Api
{
    using Entities;

    public class EntityMapper
    {
        public static Entity Map(Entity entity, Resource<int?> resource)
        {
            if (resource.Id.HasValue)
                entity.Id = resource.Id.Value;

            if (resource.RowVersion != null)
                entity.RowVersion = resource.RowVersion;

            if (resource.CreatedBy != null)
                entity.CreatedBy = resource.CreatedBy;

            if (resource.ModifiedBy != null)
                entity.ModifiedBy = resource.ModifiedBy;

            return entity;
        }
    }
}
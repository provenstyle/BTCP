namespace BibleTraining.Api
{
    using Entities;

    public class ResourceMapper
    {
        public static Resource<int?> Map(Resource<int?> resource, Entity entity)
        {
            if (entity == null) return null;

            resource.Id         = entity.Id;
            resource.RowVersion = entity.RowVersion;
            resource.CreatedBy  = entity.CreatedBy;
            resource.ModifiedBy = entity.ModifiedBy;

            return resource;
        }
    }
}
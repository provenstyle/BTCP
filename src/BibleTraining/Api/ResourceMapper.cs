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
            resource.Created    = entity.Created;
            resource.CreatedBy  = entity.CreatedBy;
            resource.Modified   = entity.Modified;
            resource.ModifiedBy = entity.ModifiedBy;

            return resource;
        }
    }
}
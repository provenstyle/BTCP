namespace BibleTraining.Maps
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;
    using Entities;

    public abstract class BaseMap<T> : EntityTypeConfiguration<T> where T: Entity
    {
        public const string DateTime2 = "DateTime2";
        protected BaseMap()
        {
            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.CreatedBy).HasMaxLength(200);
            Property(x => x.ModifiedBy).HasMaxLength(200);
            Property(x => x.Created).HasColumnType(DateTime2);
            Property(x => x.Modified).HasColumnType(DateTime2);
            Property(x => x.RowVersion).HasColumnType("ROWVERSION");
        }
    }
}
namespace $ApplicationName$.EntityTypeConfigurations
{
	using Entities;

    public class $Entity$EntityTypeConfiguration : BaseEntityTypeConfiguration<$Entity$>
    {
        public $Entity$EntityTypeConfiguration()
        {
            ToTable(nameof($Entity$));
        }
    }
}

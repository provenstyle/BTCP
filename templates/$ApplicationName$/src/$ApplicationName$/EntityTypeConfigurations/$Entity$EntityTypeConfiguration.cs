namespace $ApplicationName$.EntityTypeConfigurations
{
	using Entity;

    public class $Entity$EntityTypeConfiguration : BaseEntityTypeConfiguration<$Entity$>
    {
        public $Entity$EntityTypeConfiguration ()
        {
            ToTable(nameof($Entity$));
        }
    }
}

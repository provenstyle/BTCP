namespace BibleTraining
{
    using System.Data.Entity;
    using Highway.Data;

    public class MappingConfiguration: IMappingConfiguration
    {
        public void ConfigureModelBuilder(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.AddFromAssembly(GetType().Assembly);
        }
    }
}

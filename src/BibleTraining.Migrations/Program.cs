namespace BibleTraining.Migrations
{
    using System;
    using System.Configuration;
    using System.Linq;
    using System.Reflection;
    using Improving.DbUp;

    internal class Program
    {
        private static readonly string[] ConfigurationVariables =
        {
            "DbName","DatabaseLocation", "LogLocation", "Env"
        };

        private static int Main()
        {
            const string connectionStringName = "BibleTrainingDomain";
            var scriptVariables = ConfigurationVariables.ToDictionary(s => s, s => ConfigurationManager.AppSettings[s]);
            var env = EnvParser.Parse(scriptVariables["Env"]);
            var shouldSeedData = env == Env.LOCAL;

            var dbName = ConfigurationManager.AppSettings["DbName"];
            var dbUpdater = new DbUpdater(Assembly.GetExecutingAssembly(), "Scripts", dbName, connectionStringName, scriptVariables, shouldSeedData, env);
            var result = dbUpdater.Run();

            #if DEBUG
                Console.WriteLine("Press any key to continue:");
                Console.ReadKey();
            #endif


            return result ? 0 : -1;
        }
    }
}

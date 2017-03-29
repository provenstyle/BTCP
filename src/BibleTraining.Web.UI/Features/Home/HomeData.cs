namespace BibleTraining.Web.UI.Features.Home
{
    using Features;
    using NLog;

    public class HomeData: BaseData
    {
        public HomeData(IBibleTrainingConfig config)
            : base(config)
        {
            ApplicationName = GlobalDiagnosticsContext.Get("ApplicationName");
            DataApiBaseUrl  = config.DataApiBaseUrl;
        }

        public string ApplicationName { get; set; }
        public string Locale          { get; set; }
        public string ClientIP        { get; set; }
        public string DataApiBaseUrl  { get; set; }
    }
}
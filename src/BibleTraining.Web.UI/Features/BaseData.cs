namespace BibleTraining.Web.UI.Features
{
    using System.Threading;
    using Home;

    public class BaseData
    {
        public BaseData(IBibleTrainingConfig config)
        {
            BaseUrl  = config.BaseUrl;
            UserName = Thread.CurrentPrincipal.Identity.Name;
            Version  = VersionHelper.VersionNumber;
        }

        public string BaseUrl  { get; set; }
        public string UserName { get; set; }
        public string Version  { get; set; }
    }
}
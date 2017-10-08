namespace BibleTraining.Web.UI.Features.Home
{
    using System.Linq;
    using System.Web.Mvc;
    using Miruken.AspNet;

    public class HomeController : ContextualController
    {
        private readonly IBibleTrainingConfig _config;

        public HomeController(IBibleTrainingConfig config)
        {
            _config = config;
        }

        [HttpGet, ActionName("Index")]
        public ActionResult GetView()
        {
            return View("Index", new HomeData(_config)
            {
                ClientIP = Request?.UserHostAddress ?? string.Empty,
                Locale   = Request?.UserLanguages?.FirstOrDefault() ?? "en-US"
            });
        }
    }
}
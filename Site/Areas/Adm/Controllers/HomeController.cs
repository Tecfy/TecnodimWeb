using System.Web.Mvc;

namespace Site.Areas.Adm.Controllers
{
    [Authorize(Roles = "Administrador")]
    [RoutePrefix("Adm/Home")]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
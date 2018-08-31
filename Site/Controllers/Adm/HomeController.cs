using System.Web.Mvc;

namespace Site.Adm.Controllers
{
    [Authorize(Roles = "Administrador")]
    [RoutePrefix("Home")]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
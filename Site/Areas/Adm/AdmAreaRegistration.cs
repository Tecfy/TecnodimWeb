using System.Web.Mvc;

namespace Site.Areas.Adm
{
    public class AdmAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Adm";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                name: "Adm_default",
                url: "Adm/{controller}/{action}/{id}",
                defaults: new { action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "Site.Areas.Adm.Controllers" }
            );
        }
    }
}
using System.Web.Http;

namespace Tecnodim
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
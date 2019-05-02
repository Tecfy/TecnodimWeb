using System.Web;
using System.Web.Configuration;

namespace Helper.ServerMap
{
    public static class ServerMapHelper
    {
        public static string GetServerMap(string appPath)
        {
            //string location = HttpContext.Current.Server.MapPath("~");

            //location = location.Substring(0, location.Length - 1);
            //location = location.Substring(0, location.LastIndexOf("\\"));

            string location = WebConfigurationManager.AppSettings["Path"];

            location = location + appPath;

            return location;
        }
    }
}

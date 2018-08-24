namespace Helper.Utility
{
    public class AppSettings
    {
        public string this[string key]
        {
            get
            {
                return System.Web.Configuration.WebConfigurationManager.AppSettings[key];
            }
        }
    }
}

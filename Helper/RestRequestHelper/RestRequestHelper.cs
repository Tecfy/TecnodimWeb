using RestSharp;
using System.Web.Configuration;

namespace Helper.RestRequestHelper
{
    public class RestRequestHelper
    {
        public static RestRequest Get(Method method, string json = null)
        {
            var request = new RestRequest(method);
            request.AddHeader("content-type", "application/json");
            request.AddHeader("X-ApiKey", WebConfigurationManager.AppSettings["Helper.RestRequestHelper.Get.ApiKey"].ToString());
            request.AddHeader("X-ApiUser", WebConfigurationManager.AppSettings["Helper.RestRequestHelper.Get.ApiUser"].ToString());
            if (method == Method.GET)
            {
                request.AddParameter("application/json", "{}", ParameterType.RequestBody);
            }
            else if (method == Method.POST || method == Method.PUT)
            {
                request.AddParameter("application/json", json, ParameterType.RequestBody);
            }

            return request;
        }
    }
}

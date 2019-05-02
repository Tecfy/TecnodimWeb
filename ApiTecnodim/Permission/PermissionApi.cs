using Helper.RestRequestHelper;
using Model.Out;
using RestSharp;
using System;
using System.Linq;
using System.Web.Configuration;

namespace ApiTecnodim
{
    public partial class PermissionApi
    {
        #region .: GET :.

        public PermissionsOut GetPermissions()
        {
            try
            {
                var client = new RestClient(WebConfigurationManager.AppSettings["ApiTecnodim.URL"].ToString() + string.Format(WebConfigurationManager.AppSettings["ApiTecnodim.PermissionApi.GetECMPermissions"].ToString()));

                var request = RestRequestHelper.Get(Method.GET);

                client.Timeout = (1000 * 60 * 60);
                client.ReadWriteTimeout = (1000 * 60 * 60);

                IRestResponse response = client.Execute(request);

                PermissionsOut PermissionsOut = SimpleJson.SimpleJson.DeserializeObject<PermissionsOut>(response.Content);

                if (!PermissionsOut.success)
                {
                    throw new Exception(PermissionsOut.messages.FirstOrDefault());
                }

                return PermissionsOut;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion
    }
}

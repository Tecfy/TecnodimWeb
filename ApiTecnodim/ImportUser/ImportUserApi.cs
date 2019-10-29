using Helper.RestRequestHelper;
using Model.In;
using Model.Out;
using RestSharp;
using System;
using System.Linq;
using System.Web.Configuration;

namespace ApiTecnodim
{
    public partial class ImportUserApi
    {
        #region .: Posts :.

        public ImportUserOut PostECMUserPermission(ImportUserIn importUserIn)
        {
            try
            {
                var client = new RestClient(WebConfigurationManager.AppSettings["ApiTecnodim.URL"].ToString() + WebConfigurationManager.AppSettings["ApiTecnodim.ImportUserApi.PostECMUserPermission"].ToString());

                var request = RestRequestHelper.Get(Method.POST, SimpleJson.SimpleJson.SerializeObject(importUserIn));

                client.Timeout = (1000 * 60 * 60);
                client.ReadWriteTimeout = (1000 * 60 * 60);

                IRestResponse response = client.Execute(request);

                ImportUserOut importUserOut = SimpleJson.SimpleJson.DeserializeObject<ImportUserOut>(response.Content);

                if (!importUserOut.success)
                {
                    throw new Exception(importUserOut.messages.FirstOrDefault());
                }

                return importUserOut;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion
    }
}

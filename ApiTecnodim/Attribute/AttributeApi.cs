using Helper.RestRequestHelper;
using Model.In;
using Model.Out;
using RestSharp;
using System;
using System.Linq;
using System.Web.Configuration;

namespace ApiTecnodim
{
    public partial class AttributeApi
    {
        public ECMAttributeOut PostECMAttributeUpdate(ECMAttributeIn ecmAttributeIn)
        {
            try
            {
                var client = new RestClient(WebConfigurationManager.AppSettings["ApiTecnodim.URL"].ToString() + WebConfigurationManager.AppSettings["ApiTecnodim.AttributeApi.PostECMAttributeUpdate"].ToString());

                var request = RestRequestHelper.Get(Method.POST, SimpleJson.SimpleJson.SerializeObject(ecmAttributeIn));

                client.Timeout = (1000 * 60 * 60);
                client.ReadWriteTimeout = (1000 * 60 * 60);

                IRestResponse response = client.Execute(request);

                ECMAttributeOut ecmAttributeOut = SimpleJson.SimpleJson.DeserializeObject<ECMAttributeOut>(response.Content);

                if (!ecmAttributeOut.success)
                {
                    throw new Exception(ecmAttributeOut.messages.FirstOrDefault());
                }

                return ecmAttributeOut;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

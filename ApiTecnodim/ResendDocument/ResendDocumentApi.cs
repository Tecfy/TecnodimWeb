using Helper.RestRequestHelper;
using Model.Out;
using RestSharp;
using System;
using System.Linq;
using System.Web.Configuration;

namespace ApiTecnodim
{
    public partial class ResendDocumentApi
    {
        #region .: GET :.

        public ResendDocumentsOut GetResendDocuments(string registration, string unity)
        {
            try
            {
                var client = new RestClient(WebConfigurationManager.AppSettings["ApiTecnodim.URL"].ToString() + string.Format(WebConfigurationManager.AppSettings["ApiTecnodim.ResendDocumentApi.GetECMResendDocuments"].ToString(), registration, unity));

                var request = RestRequestHelper.Get(Method.GET);

                client.Timeout = (1000 * 60 * 60);
                client.ReadWriteTimeout = (1000 * 60 * 60);

                IRestResponse response = client.Execute(request);

                ResendDocumentsOut resendDocumentsOut = SimpleJson.SimpleJson.DeserializeObject<ResendDocumentsOut>(response.Content);

                if (!resendDocumentsOut.success)
                {
                    throw new Exception(resendDocumentsOut.messages.FirstOrDefault());
                }

                return resendDocumentsOut;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion
    }
}

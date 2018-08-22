using Helper.RestRequestHelper;
using Model.Out;
using RestSharp;
using System;
using System.Linq;
using System.Web.Configuration;

namespace ApiTecnodim
{
    public partial class DocumentApi
    {
        public ECMDocumentOut GetECMDocument(string externalId)
        {
            try
            {
                var client = new RestClient(WebConfigurationManager.AppSettings["ApiTecnodim.URL"].ToString() + string.Format(WebConfigurationManager.AppSettings["ApiTecnodim.DocumentApi.GetECMDocument"].ToString(), externalId));

                var request = RestRequestHelper.Get(Method.GET);

                IRestResponse response = client.Execute(request);

                ECMDocumentOut ecmDocumentOut = SimpleJson.SimpleJson.DeserializeObject<ECMDocumentOut>(response.Content);

                if (!ecmDocumentOut.success)
                {
                    throw new Exception(ecmDocumentOut.messages.FirstOrDefault());
                }

                return ecmDocumentOut;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public ECMDocumentsOut GetECMDocuments()
        {
            try
            {
                var client = new RestClient(WebConfigurationManager.AppSettings["ApiTecnodim.URL"].ToString() + WebConfigurationManager.AppSettings["ApiTecnodim.DocumentApi.GetECMDocuments"].ToString());

                var request = RestRequestHelper.Get(Method.GET);

                IRestResponse response = client.Execute(request);

                ECMDocumentsOut ecmDocumentsOut = SimpleJson.SimpleJson.DeserializeObject<ECMDocumentsOut>(response.Content);

                if (!ecmDocumentsOut.success)
                {
                    throw new Exception(ecmDocumentsOut.messages.FirstOrDefault());
                }

                return ecmDocumentsOut;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

using Helper.RestRequestHelper;
using Model.In;
using Model.Out;
using RestSharp;
using System;
using System.Linq;
using System.Web.Configuration;

namespace ApiTecnodim
{
    public partial class DocumentApi
    {
        #region .: Gets :.

        public ECMDocumentOut GetECMDocument(string externalId)
        {
            try
            {
                var client = new RestClient(WebConfigurationManager.AppSettings["ApiTecnodim.URL"].ToString() + string.Format(WebConfigurationManager.AppSettings["ApiTecnodim.DocumentApi.GetECMDocument"].ToString(), externalId));

                var request = RestRequestHelper.Get(Method.GET);

                client.Timeout = (1000 * 60 * 60);
                client.ReadWriteTimeout = (1000 * 60 * 60);

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

                client.Timeout = (1000 * 60 * 60);
                client.ReadWriteTimeout = (1000 * 60 * 60);

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

        #endregion

        #region .: Posts :.

        public ECMDocumentSaveOut PostECMDocumentSave(ECMDocumentSaveIn ecmDocumentSaveIn)
        {
            try
            {
                string uri = WebConfigurationManager.AppSettings["ApiTecnodim.URL"].ToString() + WebConfigurationManager.AppSettings["ApiTecnodim.DocumentApi.PostECMDocumentSave"].ToString();

                var client = new RestClient(uri);

                var x = SimpleJson.SimpleJson.SerializeObject(ecmDocumentSaveIn).Length;

                var request = RestRequestHelper.Get(Method.POST, SimpleJson.SimpleJson.SerializeObject(ecmDocumentSaveIn));

                client.Timeout = (1000 * 60 * 60);
                client.ReadWriteTimeout = (1000 * 60 * 60);

                IRestResponse response = client.Execute(request);

                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    throw new Exception(string.Format(" URI: {0}. Message: {1}. Length: {2}", uri, response.ErrorException != null ? response.ErrorException.Message : "NA", x));
                }

                ECMDocumentSaveOut ecmDocumentSaveOut = SimpleJson.SimpleJson.DeserializeObject<ECMDocumentSaveOut>(response.Content);

                if (!ecmDocumentSaveOut.success)
                {
                    throw new Exception(ecmDocumentSaveOut.messages.FirstOrDefault());
                }

                return ecmDocumentSaveOut;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region .: Delete :.

        public ECMDocumentDeleteOut DeleteECMDocument(string externalId)
        {
            try
            {
                var client = new RestClient(WebConfigurationManager.AppSettings["ApiTecnodim.URL"].ToString() + string.Format(WebConfigurationManager.AppSettings["ApiTecnodim.DocumentApi.DeleteECMDocument"].ToString(), externalId));

                var request = RestRequestHelper.Get(Method.DELETE);

                client.Timeout = (1000 * 60 * 60);
                client.ReadWriteTimeout = (1000 * 60 * 60);

                IRestResponse response = client.Execute(request);

                ECMDocumentDeleteOut eCMDocumentDeleteOut = SimpleJson.SimpleJson.DeserializeObject<ECMDocumentDeleteOut>(response.Content);

                if (!eCMDocumentDeleteOut.success)
                {
                    throw new Exception(eCMDocumentDeleteOut.messages.FirstOrDefault());
                }

                return eCMDocumentDeleteOut;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion
    }
}

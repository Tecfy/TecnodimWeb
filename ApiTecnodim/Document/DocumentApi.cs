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

        #endregion

        #region .: Posts :.

        public ECMDocumentSaveOut PostECMDocumentSave(ECMDocumentSaveIn ecmDocumentSaveIn)
        {
            try
            {
                var client = new RestClient(WebConfigurationManager.AppSettings["ApiTecnodim.URL"].ToString() + WebConfigurationManager.AppSettings["ApiTecnodim.DocumentApi.PostECMDocumentSave"].ToString());

                var request = RestRequestHelper.Get(Method.POST, SimpleJson.SimpleJson.SerializeObject(ecmDocumentSaveIn));

                IRestResponse response = client.Execute(request);

                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    throw new Exception(i18n.Resource.UnknownError);
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

        public ECMDocumentDeletedOut DeleteECMDocumentArchive(ECMDocumentDeletedIn ecmDocumentDeletedIn)
        {
            try
            {
                var client = new RestClient(WebConfigurationManager.AppSettings["ApiTecnodim.URL"].ToString() + WebConfigurationManager.AppSettings["ApiTecnodim.DocumentApi.DeleteECMDocumentArchive"].ToString());

                var request = RestRequestHelper.Get(Method.POST, SimpleJson.SimpleJson.SerializeObject(ecmDocumentDeletedIn));

                IRestResponse response = client.Execute(request);

                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    throw new Exception(i18n.Resource.UnknownError);
                }

                ECMDocumentDeletedOut ecmDocumentDeletedOut = SimpleJson.SimpleJson.DeserializeObject<ECMDocumentDeletedOut>(response.Content);

                if (!ecmDocumentDeletedOut.success)
                {
                    throw new Exception(ecmDocumentDeletedOut.messages.FirstOrDefault());
                }

                return ecmDocumentDeletedOut;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion
    }
}

using Helper.RestRequestHelper;
using Model.In;
using Model.Out;
using RestSharp;
using System;
using System.Linq;
using System.Web.Configuration;

namespace ApiTecnodim
{
    public partial class DocumentDetailApi
    {
        #region .: GET :.

        public DocumentDetailsByRegistrationOut GetDocumentDetailsByRegistration(string registration, string unity)
        {
            try
            {
                var client = new RestClient(WebConfigurationManager.AppSettings["ApiTecnodim.URL"].ToString() + string.Format(WebConfigurationManager.AppSettings["ApiTecnodim.DocumentDetailApi.GetECMDocumentDetailsByRegistration"].ToString(), registration, unity));

                var request = RestRequestHelper.Get(Method.GET);

                client.Timeout = (1000 * 60 * 60);
                client.ReadWriteTimeout = (1000 * 60 * 60);

                IRestResponse response = client.Execute(request);

                DocumentDetailsByRegistrationOut documentDetailsByRegistrationOut = SimpleJson.SimpleJson.DeserializeObject<DocumentDetailsByRegistrationOut>(response.Content);

                if (!documentDetailsByRegistrationOut.success)
                {
                    throw new Exception(documentDetailsByRegistrationOut.messages.FirstOrDefault());
                }

                return documentDetailsByRegistrationOut;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public DocumentDetailJobIdOut GetECMDocumentDetailByRegistration(string registration, string unity)
        {
            try
            {
                var client = new RestClient(WebConfigurationManager.AppSettings["ApiTecnodim.URL"].ToString() + string.Format(WebConfigurationManager.AppSettings["ApiTecnodim.DocumentDetailApi.GetECMDocumentDetailByRegistration"].ToString(), registration, unity));

                var request = RestRequestHelper.Get(Method.GET);

                client.Timeout = (1000 * 60 * 60);
                client.ReadWriteTimeout = (1000 * 60 * 60);

                IRestResponse response = client.Execute(request);

                DocumentDetailJobIdOut documentDetailJobIdOut = SimpleJson.SimpleJson.DeserializeObject<DocumentDetailJobIdOut>(response.Content);

                if (!documentDetailJobIdOut.success)
                {
                    throw new Exception(documentDetailJobIdOut.messages.FirstOrDefault());
                }

                return documentDetailJobIdOut;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public DocumentDetailDocumentIdOut GetDocumentDetailDocumentId(string registration)
        {
            try
            {
                var client = new RestClient(WebConfigurationManager.AppSettings["ApiTecnodim.URL"].ToString() + string.Format(WebConfigurationManager.AppSettings["ApiTecnodim.DocumentDetailApi.GetECMDocumentDetail"].ToString(), registration));

                var request = RestRequestHelper.Get(Method.GET);

                client.Timeout = (1000 * 60 * 60);
                client.ReadWriteTimeout = (1000 * 60 * 60);

                IRestResponse response = client.Execute(request);

                DocumentDetailDocumentIdOut documentDetailDocumentIdOut = SimpleJson.SimpleJson.DeserializeObject<DocumentDetailDocumentIdOut>(response.Content);

                if (!documentDetailDocumentIdOut.success)
                {
                    throw new Exception(documentDetailDocumentIdOut.messages.FirstOrDefault());
                }

                return documentDetailDocumentIdOut;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion
    }
}

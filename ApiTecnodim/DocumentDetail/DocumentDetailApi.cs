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

        public DocumentDetailOut GetDocumentDetail(string registration)
        {
            try
            {
                var client = new RestClient(WebConfigurationManager.AppSettings["ApiTecnodim.URL"].ToString() + string.Format(WebConfigurationManager.AppSettings["ApiTecnodim.DocumentDetailApi.GetECMDocumentDetail"].ToString(), registration));

                var request = RestRequestHelper.Get(Method.GET);

                IRestResponse response = client.Execute(request);

                DocumentDetailOut documentDetailOut = SimpleJson.SimpleJson.DeserializeObject<DocumentDetailOut>(response.Content);

                if (!documentDetailOut.success)
                {
                    throw new Exception(documentDetailOut.messages.FirstOrDefault());
                }

                return documentDetailOut;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region .: Posts :.

        public ECMDocumentDetailSaveOut PostECMDocumentDetailSave(ECMDocumentDetailSaveIn eCMDocumentDetailSaveIn)
        {
            try
            {
                var client = new RestClient(WebConfigurationManager.AppSettings["ApiTecnodim.URL"].ToString() + WebConfigurationManager.AppSettings["ApiTecnodim.DocumentDetailApi.PostECMDocumentDetailSave"].ToString());

                var request = RestRequestHelper.Get(Method.POST, SimpleJson.SimpleJson.SerializeObject(eCMDocumentDetailSaveIn));

                IRestResponse response = client.Execute(request);

                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    throw new Exception(i18n.Resource.UnknownError);
                }

                ECMDocumentDetailSaveOut eCMDocumentDetailSaveOut = SimpleJson.SimpleJson.DeserializeObject<ECMDocumentDetailSaveOut>(response.Content);

                if (!eCMDocumentDetailSaveOut.success)
                {
                    throw new Exception(eCMDocumentDetailSaveOut.messages.FirstOrDefault());
                }

                return eCMDocumentDetailSaveOut;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion
    }
}

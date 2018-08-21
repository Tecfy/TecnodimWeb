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
        public DocumentOut GetDocument(string externalId)
        {
            try
            {
                var client = new RestClient(WebConfigurationManager.AppSettings["ApiTecnodim.URL"].ToString() + string.Format(WebConfigurationManager.AppSettings["ApiTecnodim.Document.GetECMDocument"].ToString(), externalId));

                var request = RestRequestHelper.Get(Method.GET);

                IRestResponse response = client.Execute(request);

                DocumentOut documentOut = SimpleJson.SimpleJson.DeserializeObject<DocumentOut>(response.Content);

                if (!documentOut.success)
                {
                    throw new Exception(documentOut.messages.FirstOrDefault());
                }

                return documentOut;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

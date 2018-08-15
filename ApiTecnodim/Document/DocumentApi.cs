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
        #region .: Custom Methods :.

        public DocumentOut GetDocument(DocumentIn documentIn)
        {
            try
            {
                var client = new RestClient(WebConfigurationManager.AppSettings["ApiTecnodim.URL"].ToString() + string.Format(WebConfigurationManager.AppSettings["ApiTecnodim.Document.GetDocument"].ToString(), documentIn.externalId.ToString()));

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

        public DocumentsOut GetDocuments(DocumentsIn documentsIn)
        {
            try
            {
                var client = new RestClient(WebConfigurationManager.AppSettings["ApiTecnodim.URL"].ToString() + WebConfigurationManager.AppSettings["ApiTecnodim.Document.GetDocuments"].ToString());

                var request = RestRequestHelper.Get(Method.GET);

                IRestResponse response = client.Execute(request);

                DocumentsOut documentsOut = SimpleJson.SimpleJson.DeserializeObject<DocumentsOut>(response.Content);

                if (!documentsOut.success)
                {
                    throw new Exception(documentsOut.messages.FirstOrDefault());
                }

                return documentsOut;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion
    }
}

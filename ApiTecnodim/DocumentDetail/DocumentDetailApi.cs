﻿using Helper.RestRequestHelper;
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
    }
}

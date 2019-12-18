using Helper.RestRequestHelper;
using Model.In;
using Model.Out;
using RestSharp;
using System;
using System.Linq;
using System.Web.Configuration;

namespace ApiTecnodim
{
    public partial class JobCategoryApi
    {   
        #region .: Posts :.

        public ECMJobSaveOut PostECMJobSave(ECMJobSaveIn eCMJobSaveIn)
        {
            try
            {
                var client = new RestClient(WebConfigurationManager.AppSettings["ApiTecnodim.URL"].ToString() + WebConfigurationManager.AppSettings["ApiTecnodim.JobCategoryApi.PostECMJobSave"].ToString());

                var request = RestRequestHelper.Get(Method.POST, SimpleJson.SimpleJson.SerializeObject(eCMJobSaveIn));

                client.Timeout = (1000 * 60 * 60);
                client.ReadWriteTimeout = (1000 * 60 * 60);

                IRestResponse response = client.Execute(request);

                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    throw new Exception(string.Format("StatusCode: {0}. ErrorMessage: {1}.", response.StatusCode, response.ErrorMessage));
                }

                ECMJobSaveOut eCMJobSaveOut = SimpleJson.SimpleJson.DeserializeObject<ECMJobSaveOut>(response.Content);

                if (!eCMJobSaveOut.success)
                {
                    throw new Exception(eCMJobSaveOut.messages.FirstOrDefault());
                }

                return eCMJobSaveOut;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion
    }
}

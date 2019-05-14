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
        #region .: Gets :.

        public ECMJobCategoryOut GetECMJobCategory(string externalId)
        {
            try
            {
                var client = new RestClient(WebConfigurationManager.AppSettings["ApiTecnodim.URL"].ToString() + string.Format(WebConfigurationManager.AppSettings["ApiTecnodim.JobCategoryApi.GetECMJobCategory"].ToString(), externalId));

                var request = RestRequestHelper.Get(Method.GET);

                client.Timeout = (1000 * 60 * 60);
                client.ReadWriteTimeout = (1000 * 60 * 60);

                IRestResponse response = client.Execute(request);

                ECMJobCategoryOut eCMJobCategoryOut = SimpleJson.SimpleJson.DeserializeObject<ECMJobCategoryOut>(response.Content);

                if (!eCMJobCategoryOut.success)
                {
                    throw new Exception(eCMJobCategoryOut.messages.FirstOrDefault());
                }

                return eCMJobCategoryOut;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region .: Posts :.

        public ECMJobCategorySaveOut SetECMJobCategorySave(ECMJobCategorySaveIn ecmJobCategorySaveIn)
        {
            try
            {
                var client = new RestClient(WebConfigurationManager.AppSettings["ApiTecnodim.URL"].ToString() + WebConfigurationManager.AppSettings["ApiTecnodim.JobCategoryApi.SetECMJobCategorySave"].ToString());

                var request = RestRequestHelper.Get(Method.POST, SimpleJson.SimpleJson.SerializeObject(ecmJobCategorySaveIn));

                client.Timeout = (1000 * 60 * 60);
                client.ReadWriteTimeout = (1000 * 60 * 60);

                IRestResponse response = client.Execute(request);

                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    throw new Exception(string.Format("StatusCode: {0}. ErrorMessage: {1}.", response.StatusCode, response.ErrorMessage));
                }

                ECMJobCategorySaveOut ecmJobCategorySaveOut = SimpleJson.SimpleJson.DeserializeObject<ECMJobCategorySaveOut>(response.Content);

                if (!ecmJobCategorySaveOut.success)
                {
                    throw new Exception(ecmJobCategorySaveOut.messages.FirstOrDefault());
                }

                return ecmJobCategorySaveOut;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

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

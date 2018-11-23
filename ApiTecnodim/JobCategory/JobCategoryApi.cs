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

        #endregion

        #region .: Posts :.

        public ECMJobCategorySaveOut PostECMJobCategorySave(ECMJobCategorySaveIn ecmJobCategorySaveIn)
        {
            try
            {
                var client = new RestClient(WebConfigurationManager.AppSettings["ApiTecnodim.URL"].ToString() + WebConfigurationManager.AppSettings["ApiTecnodim.JobCategoryApi.PostECMJobCategorySave"].ToString());

                var request = RestRequestHelper.Get(Method.POST, SimpleJson.SimpleJson.SerializeObject(ecmJobCategorySaveIn));

                IRestResponse response = client.Execute(request);

                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    throw new Exception(i18n.Resource.UnknownError);
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

        #endregion
    }
}

using Helper.RestRequestHelper;
using Model.In;
using Model.Out;
using RestSharp;
using System;
using System.Linq;
using System.Web.Configuration;

namespace ApiTecnodim
{
    public partial class WorkCategoryApi
    {
        #region .: Gets :.

        #endregion

        #region .: Posts :.

        public ECMWorkCategorySaveOut PostECMWorkCategorySave(ECMWorkCategorySaveIn ecmWorkCategorySaveIn)
        {
            try
            {
                var client = new RestClient(WebConfigurationManager.AppSettings["ApiTecnodim.URL"].ToString() + WebConfigurationManager.AppSettings["ApiTecnodim.WorkCategoryApi.PostECMWorkCategorySave"].ToString());

                var request = RestRequestHelper.Get(Method.POST, SimpleJson.SimpleJson.SerializeObject(ecmWorkCategorySaveIn));

                IRestResponse response = client.Execute(request);

                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    throw new Exception(i18n.Resource.UnknownError);
                }

                ECMWorkCategorySaveOut ecmWorkCategorySaveOut = SimpleJson.SimpleJson.DeserializeObject<ECMWorkCategorySaveOut>(response.Content);

                if (!ecmWorkCategorySaveOut.success)
                {
                    throw new Exception(ecmWorkCategorySaveOut.messages.FirstOrDefault());
                }

                return ecmWorkCategorySaveOut;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion
    }
}

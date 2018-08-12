using Helper.RestRequestHelper;
using Model.In;
using Model.Out;
using RestSharp;
using System;
using System.Linq;
using System.Web.Configuration;

namespace ApiTecnodim
{
    public partial class CategoryApi
    {
        #region .: Custom Methods :.

        public CategoriesOut GetCategories(CategoriesIn categoriesIn)
        {
            try
            {
                var client = new RestClient(WebConfigurationManager.AppSettings["ApiTecnodim.URL"].ToString() + WebConfigurationManager.AppSettings["ApiTecnodim.Document.GetCategories"].ToString());

                var request = RestRequestHelper.Get(Method.GET);

                IRestResponse response = client.Execute(request);

                CategoriesOut categoriesOut = SimpleJson.SimpleJson.DeserializeObject<CategoriesOut>(response.Content);

                if (!categoriesOut.success)
                {
                    throw new Exception(categoriesOut.messages.FirstOrDefault());
                }

                return categoriesOut;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion
    }
}

﻿using Helper.RestRequestHelper;
using Model.Out;
using RestSharp;
using System;
using System.Linq;
using System.Web.Configuration;

namespace ApiTecnodim
{
    public partial class CategoryApi
    {
        public ECMCategoriesOut GetECMCategories()
        {
            try
            {
                var client = new RestClient(WebConfigurationManager.AppSettings["ApiTecnodim.URL"].ToString() + WebConfigurationManager.AppSettings["ApiTecnodim.CategoryApi.GetECMCategories"].ToString());

                var request = RestRequestHelper.Get(Method.GET);

                IRestResponse response = client.Execute(request);

                ECMCategoriesOut ecmCategoriesOut = SimpleJson.SimpleJson.DeserializeObject<ECMCategoriesOut>(response.Content);

                if (!ecmCategoriesOut.success)
                {
                    throw new Exception(ecmCategoriesOut.messages.FirstOrDefault());
                }

                return ecmCategoriesOut;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
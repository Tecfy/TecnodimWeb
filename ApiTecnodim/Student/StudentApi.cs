using Helper.RestRequestHelper;
using Model.In;
using Model.Out;
using RestSharp;
using System;
using System.Linq;
using System.Web.Configuration;

namespace ApiTecnodim
{
    public partial class StudentApi
    {
        public StudentOut GetStudent(StudentIn studentIn)
        {
            try
            {
                var client = new RestClient(WebConfigurationManager.AppSettings["ApiTecnodim.URL"].ToString() + string.Format(WebConfigurationManager.AppSettings["ApiTecnodim.Student.GetStudent"].ToString(), studentIn.externalId.ToString()));

                var request = RestRequestHelper.Get(Method.GET);

                IRestResponse response = client.Execute(request);

                StudentOut studentOut = SimpleJson.SimpleJson.DeserializeObject<StudentOut>(response.Content);

                if (!studentOut.success)
                {
                    throw new Exception(studentOut.messages.FirstOrDefault());
                }

                return studentOut;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

using Model.In;
using Repository;
using System;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Site.Api.Controllers
{
    [RoutePrefix("Api/ImportUsers")]
    public class ImportUsersController : ApiController
    {
        private RegisterEventRepository registerEventRepository = new RegisterEventRepository();
        private ImportUserRepository importUserRepository = new ImportUserRepository();

        [AllowAnonymous, HttpGet]
        public HttpResponseMessage GetImportUsers()
        {
            var currentContext = HttpContext.Current;

            System.Threading.Tasks.Task objTask = System.Threading.Tasks.Task.Factory.StartNew(() =>
            {
                HttpContext.Current = currentContext;

                string Key = Guid.NewGuid().ToString();

                try
                {
                    ImportUsersIn importUsersIn = new ImportUsersIn() { key = Key };

                    importUserRepository.GetImportUsers(importUsersIn);
                }
                catch (Exception ex)
                {
                    registerEventRepository.SaveRegisterEvent("", Key, "Erro", "Tecnodim.Controllers.ImportUsersController.GetImportUsers", ex.Message);
                }
            });

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}

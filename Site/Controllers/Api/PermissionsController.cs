using Model.In;
using Repository;
using System;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Site.Api.Controllers
{
    [RoutePrefix("Api/Permissions")]
    public class PermissionsController : ApiController
    {
        private RegisterEventRepository registerEventRepository = new RegisterEventRepository();
        private PermissionRepository permissionRepository = new PermissionRepository();

        [AllowAnonymous, HttpGet]
        public HttpResponseMessage GetPermissions()
        {
            var currentContext = HttpContext.Current;

            System.Threading.Tasks.Task objTask = System.Threading.Tasks.Task.Factory.StartNew(() =>
            {
                HttpContext.Current = currentContext;

                string Key = Guid.NewGuid().ToString();

                try
                {
                    PermissionsIn PermissionsIn = new PermissionsIn() { key = Key };

                    permissionRepository.GetPermissions(PermissionsIn);
                }
                catch (Exception ex)
                {
                    registerEventRepository.SaveRegisterEvent("", Key, "Erro", "Tecnodim.Controllers.PermissionsController.GetPermissions", ex.Message);
                }
            });

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}

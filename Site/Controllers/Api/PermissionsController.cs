using Microsoft.AspNet.Identity;
using Model.In;
using Model.Out;
using Repository;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.ModelBinding;

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

        [Authorize(Roles = "Usuário"), HttpPost, Route("")]
        public PermissionOut Post(PermissionIn permissionIn)
        {
            PermissionOut permissionOut = new PermissionOut();
            string Key = Guid.NewGuid().ToString();

            try
            {
                if (ModelState.IsValid)
                {
                    permissionIn.id = User.Identity.GetUserId();
                    permissionIn.key = Key;

                    permissionOut = permissionRepository.SetPermission(permissionIn);
                }
                else
                {
                    foreach (ModelState modelState in ModelState.Values)
                    {
                        var errors = modelState.Errors;
                        if (errors.Any())
                        {
                            foreach (ModelError error in errors)
                            {
                                throw new Exception(error.ErrorMessage);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                registerEventRepository.SaveRegisterEvent(User.Identity.GetUserId(), Key, "Erro", "Tecnodim.Controllers.PermissionsController.PostPermission", ex.Message);

                permissionOut.successMessage = null;
                permissionOut.messages.Add(ex.Message);
            }

            return permissionOut;
        }

    }
}

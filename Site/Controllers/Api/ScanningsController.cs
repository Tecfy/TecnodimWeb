using Microsoft.AspNet.Identity;
using Model.In;
using Model.Out;
using Repository;
using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.ModelBinding;

namespace Site.Api.Controllers
{
    [RoutePrefix("Api/Scannings")]
    public class ScanningsController : ApiController
    {
        private RegisterEventRepository registerEventRepository = new RegisterEventRepository();
        private ScanningRepository scanningRepository = new ScanningRepository();

        [Authorize(Roles = "Usuário"), HttpGet]
        public ScanningPermissionOut GetPermission()
        {
            ScanningPermissionOut scanningPermissionOut = new ScanningPermissionOut();
            string Key = Guid.NewGuid().ToString();

            try
            {
                ScanningPermissionIn scanningPermissionIn = new ScanningPermissionIn { id = User.Identity.GetUserId(), key = Key };

                scanningPermissionOut = scanningRepository.GetPermission(scanningPermissionIn);
            }
            catch (Exception ex)
            {
                registerEventRepository.SaveRegisterEvent("", Key, "Erro", "Tecnodim.Controllers.ScanningsController.GetPermission", ex.Message);

                scanningPermissionOut.successMessage = null;
                scanningPermissionOut.messages.Add(ex.Message);
            }

            return scanningPermissionOut;
        }

        [Authorize(Roles = "Usuário"), HttpPost, Route("")]
        public ScanningOut Post(ScanningIn scanningIn)
        {
            ScanningOut sliceOut = new ScanningOut();
            string Key = Guid.NewGuid().ToString();

            try
            {
                if (ModelState.IsValid)
                {
                    scanningIn.id = User.Identity.GetUserId();
                    scanningIn.key = Key;

                    sliceOut = scanningRepository.SaveScanning(scanningIn);
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
                registerEventRepository.SaveRegisterEvent(User.Identity.GetUserId(), Key, "Erro", "Tecnodim.Controllers.ScanningsController.Post", ex.Message);

                sliceOut.successMessage = null;
                sliceOut.messages.Add(ex.Message);
            }

            return sliceOut;
        }
    }
}

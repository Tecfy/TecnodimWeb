using Microsoft.AspNet.Identity;
using Model.In;
using Model.Out;
using Repository;
using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.ModelBinding;

namespace Tecnodim.Controllers
{
    [RoutePrefix("api/clippings")]
    public class ClippingsController : ApiController
    {
        RegisterEventRepository registerEventRepository = new RegisterEventRepository();
        ClippingRepository clippingRepository = new ClippingRepository();

        [Authorize, HttpGet, Route("")]
        public ClippingsOut Get(int documentId)
        {
            ClippingsOut clippingsOut = new ClippingsOut();
            Guid Key = Guid.NewGuid();

            try
            {
                if (ModelState.IsValid)
                {
                    ClippingsIn clippingsIn = new ClippingsIn() { documentId = documentId, userId = new Guid(User.Identity.GetUserId()), key = Key };

                    clippingsOut = clippingRepository.GetClippings(clippingsIn);
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
                registerEventRepository.SaveRegisterEvent(new Guid(User.Identity.GetUserId()), Key, "Erro", "Tecnodim.Controllers.ClippingsController.Get", ex.Message);

                clippingsOut.successMessage = null;
                clippingsOut.messages.Add(ex.Message);
            }

            return clippingsOut;
        }

        [Authorize, HttpPost, Route("")]
        public ClippingOut Post(ClippingIn clippingIn)
        {
            ClippingOut clippingOut = new ClippingOut();
            Guid Key = Guid.NewGuid();

            try
            {
                if (ModelState.IsValid)
                {
                    clippingIn.userId = new Guid(User.Identity.GetUserId());
                    clippingIn.key = Key;

                    clippingOut = clippingRepository.SaveClippings(clippingIn);
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

                return clippingOut;
            }
            catch (Exception ex)
            {
                registerEventRepository.SaveRegisterEvent(new Guid(User.Identity.GetUserId()), Key, "Erro", "Tecnodim.Controllers.ClippingsController.Post", ex.Message);

                clippingOut.successMessage = null;
                clippingOut.messages.Add(ex.Message);

                return clippingOut;
            }
        }
    }
}

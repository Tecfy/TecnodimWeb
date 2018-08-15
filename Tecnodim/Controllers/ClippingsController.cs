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

        [Authorize, HttpGet]
        public ClippingOut GetClipping(int clippingId)
        {
            ClippingOut clippingOut = new ClippingOut();
            Guid Key = Guid.NewGuid();

            try
            {
                if (ModelState.IsValid)
                {
                    ClippingIn clippingIn = new ClippingIn() { clippingId = clippingId, userId = new Guid(User.Identity.GetUserId()), key = Key };

                    clippingOut = clippingRepository.GetClipping(clippingIn);
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
                registerEventRepository.SaveRegisterEvent(new Guid(User.Identity.GetUserId()), Key, "Erro", "Tecnodim.Controllers.ClippingsController.GetClipping", ex.Message);

                clippingOut.successMessage = null;
                clippingOut.messages.Add(ex.Message);
            }

            return clippingOut;
        }

        [Authorize, HttpGet]
        public ClippingsOut GetClippings(int externalId)
        {
            ClippingsOut clippingsOut = new ClippingsOut();
            Guid Key = Guid.NewGuid();

            try
            {
                if (ModelState.IsValid)
                {
                    ClippingsIn clippingsIn = new ClippingsIn() { externalId = externalId, userId = new Guid(User.Identity.GetUserId()), key = Key };

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
                registerEventRepository.SaveRegisterEvent(new Guid(User.Identity.GetUserId()), Key, "Erro", "Tecnodim.Controllers.ClippingsController.GetClippings", ex.Message);

                clippingsOut.successMessage = null;
                clippingsOut.messages.Add(ex.Message);
            }

            return clippingsOut;
        }

        [Authorize, HttpPost, Route("")]
        public ClippingOut Post(ClippingSaveIn clippingIn)
        {
            ClippingOut clippingOut = new ClippingOut();
            Guid Key = Guid.NewGuid();

            try
            {
                if (ModelState.IsValid)
                {
                    clippingIn.userId = new Guid(User.Identity.GetUserId());
                    clippingIn.key = Key;

                    clippingOut = clippingRepository.SaveClipping(clippingIn);
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

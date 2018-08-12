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
    [RoutePrefix("api/ratings")]
    public class RatingsController : ApiController
    {
        RegisterEventRepository registerEventRepository = new RegisterEventRepository();
        RatingRepository ratingRepository = new RatingRepository();

        [Authorize, HttpPost, Route("")]
        public ClippingOut Post(RatingIn ratingIn)
        {
            ClippingOut clippingOut = new ClippingOut();
            Guid Key = Guid.NewGuid();

            try
            {
                if (ModelState.IsValid)
                {
                    ratingIn.userId = new Guid(User.Identity.GetUserId());
                    ratingIn.key = Key;

                    clippingOut = ratingRepository.SaveRating(ratingIn);
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
                registerEventRepository.SaveRegisterEvent(new Guid(User.Identity.GetUserId()), Key, "Erro", "Tecnodim.Controllers.RatingsController.Post", ex.Message);

                clippingOut.successMessage = null;
                clippingOut.messages.Add(ex.Message);
            }

            return clippingOut;
        }
    }
}

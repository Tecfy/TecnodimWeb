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
    [RoutePrefix("api/classifications")]
    public class ClassificationsController : ApiController
    {
        RegisterEventRepository registerEventRepository = new RegisterEventRepository();
        ClassificationRepository classificationRepository = new ClassificationRepository();

        [Authorize, HttpPost, Route("")]
        public ClassificationOut Post(ClassificationIn classificationIn)
        {
            ClassificationOut clippingOut = new ClassificationOut();
            Guid Key = Guid.NewGuid();

            try
            {
                if (ModelState.IsValid)
                {
                    classificationIn.userId = new Guid(User.Identity.GetUserId());
                    classificationIn.key = Key;

                    clippingOut = classificationRepository.SaveClassification(classificationIn);
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
                registerEventRepository.SaveRegisterEvent(new Guid(User.Identity.GetUserId()), Key, "Erro", "Tecnodim.Controllers.ClassificationsController.Post", ex.Message);

                clippingOut.successMessage = null;
                clippingOut.messages.Add(ex.Message);
            }

            return clippingOut;
        }
    }
}

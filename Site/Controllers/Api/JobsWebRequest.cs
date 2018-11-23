using Microsoft.AspNet.Identity;
using Model.In;
using Model.Out;
using Repository;
using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.ModelBinding;

namespace Site.Controllers.Api
{
    [RoutePrefix("Api/JobsWebRequest")]
    public class JobsWebRequestController : ApiController
    {
        RegisterEventRepository registerEventRepository = new RegisterEventRepository();
        JobRepository jobRepository = new JobRepository();

        //Function requested from websystem in order to create a new order for digitalize document
        public JobsWebRequestOut Post(JobsWebRequestIn jobswebrequestIn)
        {
            JobsWebRequestOut jobWebOut = new JobsWebRequestOut();
            string Key = Guid.NewGuid().ToString();

            try
            {
                if (ModelState.IsValid)
                {
                    jobswebrequestIn.userId = User.Identity.GetUserId();
                    jobswebrequestIn.key = Key;

                    jobWebOut = classificationRepository.SaveClassification(classificationIn);
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
                registerEventRepository.SaveRegisterEvent(User.Identity.GetUserId(), Key, "Erro", "Tecnodim.Controllers.ClassificationsController.Post", ex.Message);

                sliceOut.successMessage = null;
                sliceOut.messages.Add(ex.Message);
            }

            return sliceOut;
        }

    }
}
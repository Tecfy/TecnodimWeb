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
    [RoutePrefix("Api/JobDeletedPages")]
    public class JobDeletedPagesController : ApiController
    {
        private RegisterEventRepository registerEventRepository = new RegisterEventRepository();
        private JobDeletedPageRepository jobDeletedPageRepository = new JobDeletedPageRepository();

        [Authorize(Roles = "Usuário"), HttpPost, Route("")]
        public JobDeletedPageOut Post(JobDeletedPageIn jobDeletedPageIn)
        {
            JobDeletedPageOut jobDeletedPageOut = new JobDeletedPageOut();
            string Key = Guid.NewGuid().ToString();

            try
            {
                if (ModelState.IsValid)
                {
                    jobDeletedPageIn.id = User.Identity.GetUserId();
                    jobDeletedPageIn.key = Key;

                    jobDeletedPageOut = jobDeletedPageRepository.SaveJobDeletedPage(jobDeletedPageIn);
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
                registerEventRepository.SaveRegisterEvent(User.Identity.GetUserId(), Key, "Erro", "Tecnodim.Controllers.JobDeletedPagesController.Post", ex.Message);

                jobDeletedPageOut.successMessage = null;
                jobDeletedPageOut.messages.Add(ex.Message);
            }

            return jobDeletedPageOut;
        }
    }
}

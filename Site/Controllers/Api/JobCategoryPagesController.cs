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
    [RoutePrefix("Api/JobCategoryPages")]
    public class JobCategoryPagesController : ApiController
    {
        private RegisterEventRepository registerEventRepository = new RegisterEventRepository();
        private JobCategoryPageRepository jobCategoryPageRepository = new JobCategoryPageRepository();

        [Authorize(Roles = "Usuário"), HttpPost, Route("")]
        public JobCategoryPageDeleteOut Post(JobCategoryPageDeleteIn jobCategoryPageDeleteIn)
        {
            JobCategoryPageDeleteOut jobCategoryPageDeleteOut = new JobCategoryPageDeleteOut();
            string Key = Guid.NewGuid().ToString();

            try
            {
                if (ModelState.IsValid)
                {
                    jobCategoryPageDeleteIn.id = User.Identity.GetUserId();
                    jobCategoryPageDeleteIn.key = Key;

                    jobCategoryPageDeleteOut = jobCategoryPageRepository.DeleteJobCategoryPage(jobCategoryPageDeleteIn);
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
                registerEventRepository.SaveRegisterEvent(User.Identity.GetUserId(), Key, "Erro", "Tecnodim.Controllers.JobCategoryPagesController.Post", ex.Message);

                jobCategoryPageDeleteOut.successMessage = null;
                jobCategoryPageDeleteOut.messages.Add(ex.Message);
            }

            return jobCategoryPageDeleteOut;
        }
    }
}

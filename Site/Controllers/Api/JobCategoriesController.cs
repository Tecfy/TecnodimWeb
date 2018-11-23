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
    [RoutePrefix("Api/JobCategories")]
    public class JobCategoriesController : ApiController
    {
        RegisterEventRepository registerEventRepository = new RegisterEventRepository();
        JobCategoryRepository jobCategoryRepository = new JobCategoryRepository();

        [AllowAnonymous, HttpPost, Route("")]
        public JobCategoryOut Post(JobCategoryIn jobCategoryIn)
        {
            JobCategoryOut jobCategoryOut = new JobCategoryOut();
            string Key = Guid.NewGuid().ToString();

            try
            {
                if (ModelState.IsValid)
                {
                    jobCategoryIn.userId = User.Identity.GetUserId();
                    jobCategoryIn.key = Key;

                    jobCategoryOut = jobCategoryRepository.SaveJobCategory(jobCategoryIn);
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
                registerEventRepository.SaveRegisterEvent(User.Identity.GetUserId(), Key, "Erro", "Tecnodim.Controllers.JobCategoriesController.Post", ex.Message);

                jobCategoryOut.successMessage = null;
                jobCategoryOut.messages.Add(ex.Message);
            }

            return jobCategoryOut;
        }

    }
}
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
    [RoutePrefix("Api/JobCategories")]
    public class JobCategoriesController : ApiController
    {
        RegisterEventRepository registerEventRepository = new RegisterEventRepository();
        JobCategoryRepository jobCategoryRepository = new JobCategoryRepository();

        #region .: API :.

        #region .: Get :.

        [AllowAnonymous, HttpPost]
        public JobCategoryArchiveOut SetJobCategorySave(JobCategoryArchiveIn jobCategorySaveIn)
        {
            JobCategoryArchiveOut jobCategorySaveOut = new JobCategoryArchiveOut();
            string Key = Guid.NewGuid().ToString();

            try
            {
                if (ModelState.IsValid)
                {
                    jobCategorySaveIn.key = Key;

                    jobCategorySaveOut = jobCategoryRepository.SetJobCategorySave(jobCategorySaveIn);
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
                registerEventRepository.SaveRegisterEvent("", Key, "Erro", "Tecnodim.Controllers.JobCategoriesController.SetJobCategorySave", ex.Message);

                jobCategorySaveOut.successMessage = null;
                jobCategorySaveOut.messages.Add(ex.Message);
            }

            return jobCategorySaveOut;
        }

        [Authorize(Roles = "Usuário"), HttpGet]
        public JobCategoriesByJobIdOut GetJobCategoriesByJobId(int jobId)
        {
            JobCategoriesByJobIdOut jobCategoriesByJobIdOut = new JobCategoriesByJobIdOut();
            string Key = Guid.NewGuid().ToString();

            try
            {
                JobCategoriesByJobIdIn jobCategoriesByJobIdIn = new JobCategoriesByJobIdIn() { jobId = jobId, id = User.Identity.GetUserId(), key = Key };

                jobCategoriesByJobIdOut = jobCategoryRepository.GetJobCategoriesByJobId(jobCategoriesByJobIdIn);
            }
            catch (Exception ex)
            {
                registerEventRepository.SaveRegisterEvent(User.Identity.GetUserId(), Key, "Erro", "Tecnodim.Controllers.JobCategoriesController.GetJobCategoriesByJobId", ex.Message);

                jobCategoriesByJobIdOut.successMessage = null;
                jobCategoriesByJobIdOut.messages.Add(ex.Message);
            }

            return jobCategoriesByJobIdOut;
        }

        #endregion

        #region .: Post :.

        #endregion

        #endregion
    }
}
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

                    jobCategorySaveOut = jobCategoryRepository.SaveJobCategory(jobCategorySaveIn);
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

        [Authorize(Roles = "Usuário"), HttpPost]
        public JobCategoryDisapproveOut SetJobCategoryDisapprove(JobCategoryDisapproveIn jobCategoryDisapproveIn)
        {
            JobCategoryDisapproveOut jobCategoryDisapproveOut = new JobCategoryDisapproveOut();
            string Key = Guid.NewGuid().ToString();

            try
            {
                if (ModelState.IsValid)
                {
                    jobCategoryDisapproveIn.id = User.Identity.GetUserId();
                    jobCategoryDisapproveIn.key = Key;

                    jobCategoryDisapproveOut = jobCategoryRepository.DisapproveJobCategory(jobCategoryDisapproveIn);
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
                registerEventRepository.SaveRegisterEvent(User.Identity.GetUserId(), Key, "Erro", "Tecnodim.Controllers.JobCategoriesController.SetJobCategoryDisapprove", ex.Message);

                jobCategoryDisapproveOut.successMessage = null;
                jobCategoryDisapproveOut.messages.Add(ex.Message);
            }

            return jobCategoryDisapproveOut;
        }

        [Authorize(Roles = "Usuário"), HttpPost]
        public JobCategoryApproveOut SetJobCategoryApprove(JobCategoryApproveIn jobCategoryApproveIn)
        {
            JobCategoryApproveOut jobCategoryApproveOut = new JobCategoryApproveOut();
            string Key = Guid.NewGuid().ToString();

            try
            {
                if (ModelState.IsValid)
                {
                    jobCategoryApproveIn.id = User.Identity.GetUserId();
                    jobCategoryApproveIn.key = Key;

                    jobCategoryApproveOut = jobCategoryRepository.ApproveJobCategory(jobCategoryApproveIn);
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
                registerEventRepository.SaveRegisterEvent(User.Identity.GetUserId(), Key, "Erro", "Tecnodim.Controllers.JobCategoriesController.SetJobCategoryApprove", ex.Message);

                jobCategoryApproveOut.successMessage = null;
                jobCategoryApproveOut.messages.Add(ex.Message);
            }

            return jobCategoryApproveOut;
        }

        [Authorize(Roles = "Usuário"), HttpPost]
        public JobCategoryDeletedOut SetJobCategoryDeleted(JobCategoryDeletedIn jobCategoryDeletedIn)
        {
            JobCategoryDeletedOut jobCategoryDeletedOut = new JobCategoryDeletedOut();
            string Key = Guid.NewGuid().ToString();

            try
            {
                if (ModelState.IsValid)
                {
                    jobCategoryDeletedIn.id = User.Identity.GetUserId();
                    jobCategoryDeletedIn.key = Key;

                    jobCategoryDeletedOut = jobCategoryRepository.DeletedJobCategory(jobCategoryDeletedIn);
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
                registerEventRepository.SaveRegisterEvent(User.Identity.GetUserId(), Key, "Erro", "Tecnodim.Controllers.JobCategoriesController.SetJobCategoryDeleted", ex.Message);

                jobCategoryDeletedOut.successMessage = null;
                jobCategoryDeletedOut.messages.Add(ex.Message);
            }

            return jobCategoryDeletedOut;
        }

        [Authorize(Roles = "Usuário"), HttpPost]
        public JobCategoryIncludeOut SetJobCategoryInclude(JobCategoryIncludeIn jobCategoryIncludeIn)
        {
            JobCategoryIncludeOut jobCategoryIncludeOut = new JobCategoryIncludeOut();
            string Key = Guid.NewGuid().ToString();

            try
            {
                if (ModelState.IsValid)
                {
                    jobCategoryIncludeIn.id = User.Identity.GetUserId();
                    jobCategoryIncludeIn.key = Key;

                    jobCategoryIncludeOut = jobCategoryRepository.IncludeJobCategory(jobCategoryIncludeIn);
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
                registerEventRepository.SaveRegisterEvent(User.Identity.GetUserId(), Key, "Erro", "Tecnodim.Controllers.JobCategoriesController.SetJobCategoryInclude", ex.Message);

                jobCategoryIncludeOut.successMessage = null;
                jobCategoryIncludeOut.messages.Add(ex.Message);
            }

            return jobCategoryIncludeOut;
        }

        #endregion

        #endregion
    }
}
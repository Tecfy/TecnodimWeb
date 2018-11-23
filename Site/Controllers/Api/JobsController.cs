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
    [RoutePrefix("Api/Jobs")]
    public class JobsController : ApiController
    {
        RegisterEventRepository registerEventRepository = new RegisterEventRepository();
        JobRepository jobRepository = new JobRepository();

        [AllowAnonymous, HttpGet]
        public JobOut GetJobByCode(string code)
        {
            JobOut jobOut = new JobOut();
            string Key = Guid.NewGuid().ToString();

            try
            {
                if (ModelState.IsValid)
                {
                    JobIn jobIn = new JobIn() { code = code, key = Key };

                    jobOut = jobRepository.GetJobByCode(jobIn);
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
                registerEventRepository.SaveRegisterEvent(User.Identity.GetUserId(), Key, "Erro", "Tecnodim.Controllers.JobsController.GetJobByCode", ex.Message);

                jobOut.successMessage = null;
                jobOut.messages.Add(ex.Message);
            }

            return jobOut;
        }

        [AllowAnonymous, HttpGet]
        public JobsRegistrationOut GetJobsByRegistration(string registration)
        {
            JobsRegistrationOut jobsRegistrationOut = new JobsRegistrationOut();
            string Key = Guid.NewGuid().ToString();

            try
            {
                if (ModelState.IsValid)
                {
                    JobsRegistrationIn jobsRegistrationIn = new JobsRegistrationIn() { registration = registration, key = Key };

                    jobsRegistrationOut = jobRepository.GetJobsByRegistration(jobsRegistrationIn);
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
                registerEventRepository.SaveRegisterEvent(User.Identity.GetUserId(), Key, "Erro", "Tecnodim.Controllers.JobsController.GetJobsByRegistration", ex.Message);

                jobsRegistrationOut.successMessage = null;
                jobsRegistrationOut.messages.Add(ex.Message);
            }

            return jobsRegistrationOut;
        }

        [Authorize(Roles = "Usuário"), HttpGet]
        public JobsOut GetJobs(string code, int currentPage = 1, int qtdEntries = 50)
        {
            JobsOut jobsOut = new JobsOut();
            string Key = Guid.NewGuid().ToString();

            try
            {
                if (ModelState.IsValid)
                {
                    JobsIn jobsIn = new JobsIn() { code = code, userId = User.Identity.GetUserId(), key = Key, currentPage = currentPage, qtdEntries = qtdEntries };

                    jobsOut = jobRepository.GetJobs(jobsIn);
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
                registerEventRepository.SaveRegisterEvent(User.Identity.GetUserId(), Key, "Erro", "Tecnodim.Controllers.WorksController.GetWorkByCode", ex.Message);

                jobsOut.successMessage = null;
                jobsOut.messages.Add(ex.Message);
            }

            return jobsOut;
        }

        //Function requested from websystem in order to create a new order for digitalize document
        public ClassificationOut PostWebRequest(ClassificationIn classificationIn)
        {
            ClassificationOut sliceOut = new ClassificationOut();
            string Key = Guid.NewGuid().ToString();

            try
            {
                if (ModelState.IsValid)
                {
                    classificationIn.userId = User.Identity.GetUserId();
                    classificationIn.key = Key;

                    sliceOut = classificationRepository.SaveClassification(classificationIn);
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
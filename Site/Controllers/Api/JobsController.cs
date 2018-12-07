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
    [RoutePrefix("Api/Jobs")]
    public class JobsController : ApiController
    {
        RegisterEventRepository registerEventRepository = new RegisterEventRepository();
        JobRepository jobRepository = new JobRepository();

        #region .: API :.

        #region .: Get :.

        [AllowAnonymous, HttpGet]
        public JobsByRegistrationOut GetJobsByRegistration(string registration)
        {
            JobsByRegistrationOut jobsByRegistrationOut = new JobsByRegistrationOut();
            string Key = Guid.NewGuid().ToString();

            try
            {
                JobsByRegistrationIn jobsByRegistrationIn = new JobsByRegistrationIn() { registration = registration, key = Key };

                jobsByRegistrationOut = jobRepository.GetJobsByRegistration(jobsByRegistrationIn);
            }
            catch (Exception ex)
            {
                registerEventRepository.SaveRegisterEvent("", Key, "Erro", "Tecnodim.Controllers.JobsController.GetJobsByRegistration", ex.Message);

                jobsByRegistrationOut.successMessage = null;
                jobsByRegistrationOut.messages.Add(ex.Message);
            }

            return jobsByRegistrationOut;
        }

        [Authorize(Roles = "Usuário"), HttpGet]
        public JobsByUserOut GetJobsByUser()
        {
            JobsByUserOut jobsByUserOut = new JobsByUserOut();
            string Key = Guid.NewGuid().ToString();

            try
            {
                JobsByUserIn jobsByUserIn = new JobsByUserIn { id = User.Identity.GetUserId(), key = Key };

                jobsByUserOut = jobRepository.GetJobsByUser(jobsByUserIn);
            }
            catch (Exception ex)
            {
                registerEventRepository.SaveRegisterEvent("", Key, "Erro", "Tecnodim.Controllers.JobsController.GetJobsByUser", ex.Message);

                jobsByUserOut.successMessage = null;
                jobsByUserOut.messages.Add(ex.Message);
            }

            return jobsByUserOut;
        }
       
        #endregion

        #region .: Post :.

        [Authorize(Roles = "Usuário"), HttpPost, Route("")]
        public JobDeleteOut Post(JobDeleteIn jobDeleteIn)
        {
            JobDeleteOut jobDeleteOut = new JobDeleteOut();
            string Key = Guid.NewGuid().ToString();

            try
            {
                if (ModelState.IsValid)
                {
                    jobDeleteIn.id = User.Identity.GetUserId();
                    jobDeleteIn.key = Key;

                    jobRepository.DeleteJob(jobDeleteIn);
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
                registerEventRepository.SaveRegisterEvent(User.Identity.GetUserId(), Key, "Erro", "Tecnodim.Controllers.JobsController.Post", ex.Message);

                jobDeleteOut.successMessage = null;
                jobDeleteOut.messages.Add(ex.Message);
            }

            return jobDeleteOut;
        }

        #endregion

        #endregion
    }
}
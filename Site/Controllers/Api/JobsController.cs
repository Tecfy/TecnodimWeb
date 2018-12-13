using Microsoft.AspNet.Identity;
using Model.In;
using Model.Out;
using Repository;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ModelBinding;

namespace Site.Api.Controllers
{
    [RoutePrefix("Api/Jobs")]
    public class JobsController : ApiController
    {
        private RegisterEventRepository registerEventRepository = new RegisterEventRepository();
        private JobRepository jobRepository = new JobRepository();
        private JobStatusRepository jobStatusRepository = new JobStatusRepository();

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

        [AllowAnonymous, HttpGet]
        public HttpResponseMessage GetECMSendJobs()
        {
            System.Threading.Tasks.Task objTask = System.Threading.Tasks.Task.Factory.StartNew(() =>
            {
                string Key = Guid.NewGuid().ToString();

                try
                {
                    ECMJobsSendIn eCMJobsSendIn = new ECMJobsSendIn() { id = "", key = Key };

                    jobRepository.GetECMSendJobs(eCMJobsSendIn);
                }
                catch (Exception ex)
                {
                    registerEventRepository.SaveRegisterEvent("", Key, "Erro", "Tecnodim.Controllers.JobsController.GetECMSendJobs", ex.Message);
                }
            });

            return Request.CreateResponse(HttpStatusCode.OK);
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

        [Authorize(Roles = "Usuário"), HttpPost]
        public JobDeleteOut SetJobDelete(JobDeleteIn jobDeleteIn)
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
                registerEventRepository.SaveRegisterEvent(User.Identity.GetUserId(), Key, "Erro", "Tecnodim.Controllers.JobsController.SetJobDelete", ex.Message);

                jobDeleteOut.successMessage = null;
                jobDeleteOut.messages.Add(ex.Message);
            }

            return jobDeleteOut;
        }

        [Authorize(Roles = "Usuário"), HttpPost]
        public JobSatusOut SetJobSatus(JobSatusIn jobSatusIn)
        {
            JobSatusOut jobSatusOut = new JobSatusOut();
            string Key = Guid.NewGuid().ToString();

            try
            {
                if (ModelState.IsValid)
                {
                    jobSatusIn.id = User.Identity.GetUserId();
                    jobSatusIn.key = Key;

                    jobSatusOut = jobStatusRepository.SatusJob(jobSatusIn);
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
                registerEventRepository.SaveRegisterEvent(User.Identity.GetUserId(), Key, "Erro", "Tecnodim.Controllers.JobsController.SetJobSatus", ex.Message);

                jobSatusOut.successMessage = null;
                jobSatusOut.messages.Add(ex.Message);
            }

            return jobSatusOut;
        }

        #endregion

        #endregion
    }
}
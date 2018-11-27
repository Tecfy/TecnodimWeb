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
        public JobsRegistrationOut GetJobsByRegistration(string registration)
        {
            JobsRegistrationOut jobsRegistrationOut = new JobsRegistrationOut();
            string Key = Guid.NewGuid().ToString();

            try
            {
                JobsRegistrationIn jobsRegistrationIn = new JobsRegistrationIn() { registration = registration, key = Key };

                jobsRegistrationOut = jobRepository.GetJobsByRegistration(jobsRegistrationIn);
            }
            catch (Exception ex)
            {
                registerEventRepository.SaveRegisterEvent("", Key, "Erro", "Tecnodim.Controllers.JobsController.GetJobsByRegistration", ex.Message);

                jobsRegistrationOut.successMessage = null;
                jobsRegistrationOut.messages.Add(ex.Message);
            }

            return jobsRegistrationOut;
        }
    }
}
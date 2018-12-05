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

        #endregion
    }
}
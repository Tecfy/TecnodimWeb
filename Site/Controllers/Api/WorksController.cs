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
    [RoutePrefix("Api/Works")]
    public class WorksController : ApiController
    {
        RegisterEventRepository registerEventRepository = new RegisterEventRepository();
        WorkRepository workRepository = new WorkRepository();

        [AllowAnonymous, HttpGet]
        public WorkOut GetWorkByCode(string code)
        {
            WorkOut workOut = new WorkOut();
            string Key = Guid.NewGuid().ToString();

            try
            {
                if (ModelState.IsValid)
                {
                    WorkIn workIn = new WorkIn() { code = code, key = Key };

                    workOut = workRepository.GetWorkByCode(workIn);
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

                workOut.successMessage = null;
                workOut.messages.Add(ex.Message);
            }

            return workOut;
        }

        [AllowAnonymous, HttpGet]
        public WorksRegistrationOut GetWorksByRegistration(string registration)
        {
            WorksRegistrationOut worksRegistrationOut = new WorksRegistrationOut();
            string Key = Guid.NewGuid().ToString();

            try
            {
                if (ModelState.IsValid)
                {
                    WorksRegistrationIn worksRegistrationIn = new WorksRegistrationIn() { registration = registration, key = Key };

                    worksRegistrationOut = workRepository.GetWorksByRegistration(worksRegistrationIn);
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
                registerEventRepository.SaveRegisterEvent(User.Identity.GetUserId(), Key, "Erro", "Tecnodim.Controllers.WorksController.GetWorksByRegistration", ex.Message);

                worksRegistrationOut.successMessage = null;
                worksRegistrationOut.messages.Add(ex.Message);
            }

            return worksRegistrationOut;
        }

        [Authorize(Roles = "Usuário"), HttpGet]
        public WorksOut GetWorks(string code, int currentPage = 1, int qtdEntries = 50)
        {
            WorksOut worksOut = new WorksOut();
            string Key = Guid.NewGuid().ToString();

            try
            {
                if (ModelState.IsValid)
                {
                    WorksIn worksIn = new WorksIn() { code = code, userId = User.Identity.GetUserId(), key = Key, currentPage = currentPage, qtdEntries = qtdEntries };

                    worksOut = workRepository.GetWorks(worksIn);
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

                worksOut.successMessage = null;
                worksOut.messages.Add(ex.Message);
            }

            return worksOut;
        }

    }
}
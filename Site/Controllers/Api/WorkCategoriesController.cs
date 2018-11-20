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
    [RoutePrefix("Api/WorkCategories")]
    public class WorkCategoriesController : ApiController
    {
        RegisterEventRepository registerEventRepository = new RegisterEventRepository();
        WorkCategoryRepository workCategoryRepository = new WorkCategoryRepository();

        [AllowAnonymous, HttpPost, Route("")]
        public WorkCategoryOut Post(WorkCategoryIn workCategoryIn)
        {
            WorkCategoryOut workCategoryOut = new WorkCategoryOut();
            string Key = Guid.NewGuid().ToString();

            try
            {
                if (ModelState.IsValid)
                {
                    workCategoryIn.userId = User.Identity.GetUserId();
                    workCategoryIn.key = Key;

                    workCategoryOut = workCategoryRepository.SaveWorkCategory(workCategoryIn);
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
                registerEventRepository.SaveRegisterEvent(User.Identity.GetUserId(), Key, "Erro", "Tecnodim.Controllers.WorkCategoriesController.Post", ex.Message);

                workCategoryOut.successMessage = null;
                workCategoryOut.messages.Add(ex.Message);
            }

            return workCategoryOut;
        }
    }
}
using Microsoft.AspNet.Identity;
using Model.In;
using Model.Out;
using Repository;
using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.ModelBinding;

namespace Tecnodim.Controllers
{
    [RoutePrefix("api/categories")]
    public class CategoriesController : ApiController
    {
        RegisterEventRepository registerEventRepository = new RegisterEventRepository();
        CategoryRepository ratingRepository = new CategoryRepository();

        [Authorize, HttpGet, Route("")]
        public CategoriesOut Get()
        {
            CategoriesOut categoriesOut = new CategoriesOut();
            Guid Key = Guid.NewGuid();

            try
            {
                if (ModelState.IsValid)
                {
                    CategoriesIn categoriesIn = new CategoriesIn() { userId = new Guid(User.Identity.GetUserId()), key = Key };

                    categoriesOut = ratingRepository.GetCategories(categoriesIn);
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
                registerEventRepository.SaveRegisterEvent(new Guid(User.Identity.GetUserId()), Key, "Erro", "Tecnodim.Controllers.CategoriesController.Get", ex.Message);

                categoriesOut.successMessage = null;
                categoriesOut.messages.Add(ex.Message);
            }

            return categoriesOut;
        }
    }
}

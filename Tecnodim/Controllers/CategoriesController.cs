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
        CategoryRepository categoryRepository = new CategoryRepository();

        [Authorize, HttpGet]
        public CategoryOut GetCategory(int categoryId)
        {
            CategoryOut categoryOut = new CategoryOut();
            Guid Key = Guid.NewGuid();

            try
            {
                if (ModelState.IsValid)
                {
                    CategoryIn categoryIn = new CategoryIn() { categoryId = categoryId, userId = new Guid(User.Identity.GetUserId()), key = Key };

                    categoryOut = categoryRepository.GetCategory(categoryIn);
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
                registerEventRepository.SaveRegisterEvent(new Guid(User.Identity.Name), Key, "Erro", "Tecnodim.Controllers.CategoriesController.GetCategory", ex.Message);

                categoryOut.successMessage = null;
                categoryOut.messages.Add(i18n.Resource.UnknownError);
            }

            return categoryOut;
        }

        [Authorize, HttpGet]
        public CategoriesOut GetCategories()
        {
            CategoriesOut categoriesOut = new CategoriesOut();
            Guid Key = Guid.NewGuid();

            try
            {
                if (ModelState.IsValid)
                {
                    CategoriesIn categoriesIn = new CategoriesIn() { userId = new Guid(User.Identity.GetUserId()), key = Key };

                    categoriesOut = categoryRepository.GetCategories(categoriesIn);
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

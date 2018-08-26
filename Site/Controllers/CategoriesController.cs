using Microsoft.AspNet.Identity;
using Model.In;
using Model.Out;
using Repository;
using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.ModelBinding;

namespace Site.Controllers
{
    [RoutePrefix("Api/Categories")]
    public class CategoriesController : ApiController
    {
        RegisterEventRepository registerEventRepository = new RegisterEventRepository();
        CategoryRepository categoryRepository = new CategoryRepository();

        [Authorize(Roles = "Usuário"), HttpGet]
        public ApiCategoryOut GetCategoryById(int id)
        {
            ApiCategoryOut categoryOut = new ApiCategoryOut();
            Guid Key = Guid.NewGuid();

            try
            {
                if (ModelState.IsValid)
                {
                    ApiCategoryIn categoryIn = new ApiCategoryIn() { categoryId = id, userId = new Guid(User.Identity.GetUserId()), key = Key };

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

        [Authorize(Roles = "Usuário"), HttpGet]
        public ApiCategorySearchOut GetCategoryBySearch(string code)
        {
            ApiCategorySearchOut categorySearchOut = new ApiCategorySearchOut();
            Guid Key = Guid.NewGuid();

            try
            {
                if (ModelState.IsValid)
                {
                    ApiCategorySearchIn categorySearchIn = new ApiCategorySearchIn() { code = code, userId = new Guid(User.Identity.GetUserId()), key = Key };

                    categorySearchOut = categoryRepository.GetCategorySearch(categorySearchIn);
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
                registerEventRepository.SaveRegisterEvent(new Guid(User.Identity.Name), Key, "Erro", "Tecnodim.Controllers.CategoriesController.GetCategorySearch", ex.Message);

                categorySearchOut.successMessage = null;
                categorySearchOut.messages.Add(i18n.Resource.UnknownError);
            }

            return categorySearchOut;
        }

        [Authorize(Roles = "Usuário"), HttpGet]
        public ApiCategoriesOut GetCategories()
        {
            ApiCategoriesOut categoriesOut = new ApiCategoriesOut();
            Guid Key = Guid.NewGuid();

            try
            {
                if (ModelState.IsValid)
                {
                    ApiCategoriesIn categoriesIn = new ApiCategoriesIn() { userId = new Guid(User.Identity.GetUserId()), key = Key };

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

        [Authorize(Roles = "Usuário"), HttpGet]
        public ApiECMCategoriesOut GetECMCategories()
        {
            ApiECMCategoriesOut ecmCategoriesOut = new ApiECMCategoriesOut();
            Guid Key = Guid.NewGuid();

            try
            {
                if (ModelState.IsValid)
                {
                    ApiECMCategoriesIn ecmCategoriesIn = new ApiECMCategoriesIn() { userId = new Guid(User.Identity.GetUserId()), key = Key };

                    ecmCategoriesOut = categoryRepository.GetECMCategories(ecmCategoriesIn);
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
                registerEventRepository.SaveRegisterEvent(new Guid(User.Identity.GetUserId()), Key, "Erro", "Tecnodim.Controllers.CategoriesController.GetECMCategories", ex.Message);

                ecmCategoriesOut.successMessage = null;
                ecmCategoriesOut.messages.Add(ex.Message);
            }

            return ecmCategoriesOut;
        }
    }
}

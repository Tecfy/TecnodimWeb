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
    [RoutePrefix("Api/Categories")]
    public class CategoriesController : ApiController
    {
        private RegisterEventRepository registerEventRepository = new RegisterEventRepository();
        private CategoryRepository categoryRepository = new CategoryRepository();

        [Authorize(Roles = "Usuário"), HttpGet]
        public ApiCategoryOut GetCategoryById(int id)
        {
            ApiCategoryOut categoryOut = new ApiCategoryOut();
            string Key = Guid.NewGuid().ToString();

            try
            {
                ApiCategoryIn categoryIn = new ApiCategoryIn() { categoryId = id, id = User.Identity.GetUserId(), key = Key };

                categoryOut = categoryRepository.GetCategory(categoryIn);
            }
            catch (Exception ex)
            {
                registerEventRepository.SaveRegisterEvent(User.Identity.Name, Key, "Erro", "Tecnodim.Controllers.CategoriesController.GetCategory", ex.Message);

                categoryOut.successMessage = null;
                categoryOut.messages.Add(i18n.Resource.UnknownError);
            }

            return categoryOut;
        }

        [Authorize(Roles = "Usuário"), HttpGet]
        public ApiCategorySearchOut GetCategoryBySearch(string code)
        {
            ApiCategorySearchOut categorySearchOut = new ApiCategorySearchOut();
            string Key = Guid.NewGuid().ToString();

            try
            {
                ApiCategorySearchIn categorySearchIn = new ApiCategorySearchIn() { code = code, id = User.Identity.GetUserId(), key = Key };

                categorySearchOut = categoryRepository.GetCategorySearch(categorySearchIn);
            }
            catch (Exception ex)
            {
                registerEventRepository.SaveRegisterEvent(User.Identity.Name, Key, "Erro", "Tecnodim.Controllers.CategoriesController.GetCategorySearch", ex.Message);

                categorySearchOut.successMessage = null;
                categorySearchOut.messages.Add(i18n.Resource.UnknownError);
            }

            return categorySearchOut;
        }

        [Authorize(Roles = "Usuário"), HttpGet]
        public ApiCategoriesOut GetCategories()
        {
            ApiCategoriesOut categoriesOut = new ApiCategoriesOut();
            string Key = Guid.NewGuid().ToString();

            try
            {
                if (ModelState.IsValid)
                {
                    ApiCategoriesIn categoriesIn = new ApiCategoriesIn() { id = User.Identity.GetUserId(), key = Key };

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
                registerEventRepository.SaveRegisterEvent(User.Identity.GetUserId(), Key, "Erro", "Tecnodim.Controllers.CategoriesController.Get", ex.Message);

                categoriesOut.successMessage = null;
                categoriesOut.messages.Add(ex.Message);
            }

            return categoriesOut;
        }

        [AllowAnonymous, HttpGet]
        public HttpResponseMessage GetECMCategories()
        {
            System.Threading.Tasks.Task objTask = System.Threading.Tasks.Task.Factory.StartNew(() =>
            {
                string Key = Guid.NewGuid().ToString();

                try
                {
                    ApiECMCategoriesIn ecmCategoriesIn = new ApiECMCategoriesIn() { id = "", key = Key };

                    categoryRepository.GetECMCategories(ecmCategoriesIn);
                }
                catch (Exception ex)
                {
                    registerEventRepository.SaveRegisterEvent("", Key, "Erro", "Tecnodim.Controllers.CategoriesController.GetECMCategories", ex.Message);
                }
            });

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}

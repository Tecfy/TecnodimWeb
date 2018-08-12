using ApiTecnodim;
using DataEF.DataAccess;
using Model.In;
using Model.Out;
using System.Linq;

namespace Repository
{
    public partial class CategoryRepository
    {
        RegisterEventRepository registerEventRepository = new RegisterEventRepository();
        CategoryApi categoryApi = new CategoryApi();

        #region .: Methods :.

        public CategoriesOut GetCategories(CategoriesIn categoriesIn)
        {
            CategoriesOut categoriesOut = new CategoriesOut();
            registerEventRepository.SaveRegisterEvent(categoriesIn.userId.Value, categoriesIn.key.Value, "Log - Start", "Repository.CategoryRepository.GetCategories", "");

            categoriesOut = categoryApi.GetCategories(categoriesIn);

            registerEventRepository.SaveRegisterEvent(categoriesIn.userId.Value, categoriesIn.key.Value, "Log - End", "Repository.CategoryRepository.GetCategories", "");
            return categoriesOut;
        }

        #endregion
    }
}

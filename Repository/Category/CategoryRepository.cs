using DataEF.DataAccess;
using Model.In;
using Model.Out;
using Model.VM;
using System.Collections.Generic;
using System.Linq;

namespace Repository
{
    public class CategoryRepository
    {
        RegisterEventRepository registerEventRepository = new RegisterEventRepository();

        public CategoryOut GetCategory(CategoryIn categoryIn)
        {
            CategoryOut categoryOut = new CategoryOut();
            registerEventRepository.SaveRegisterEvent(categoryIn.userId.Value, categoryIn.key.Value, "Log - Start", "Repository.CategoryRepository.GetCategory", "");

            using (var db = new DBContext())
            {
                categoryOut.result = db.Categories
                                    .Where(x => x.DeletedDate == null && x.Active == true && x.CategoryId == categoryIn.categoryId)
                                    .Select(x => new CategoryVM()
                                    {
                                        categoryId = x.CategoryId,
                                        code = x.Code,
                                        name = x.Name,
                                        parentId = x.ParentId,
                                        additionalFields = x.CategoryAdditionalFields
                                                            .Select(y => new AdditionalFieldVM()
                                                            {
                                                                categoryAdditionalFieldId = y.CategoryAdditionalFieldId,
                                                                name = y.AdditionalFields.Name,
                                                                type = y.AdditionalFields.Type,
                                                                single = y.Single,
                                                                required = y.Required,
                                                                confidential = y.Confidential,
                                                            }).ToList()
                                    }).FirstOrDefault();
            }

            if (categoryOut.result.parentId != null)
            {
                categoryOut.result.parents = GetParents(categoryOut.result.parentId.Value, new List<string>());
                categoryOut.result.parents = categoryOut.result.parents.OrderBy(x => x).ToList();
            }

            registerEventRepository.SaveRegisterEvent(categoryIn.userId.Value, categoryIn.key.Value, "Log - End", "Repository.CategoryRepository.GetCategory", "");
            return categoryOut;
        }

        public CategoriesOut GetCategories(CategoriesIn categoriesIn)
        {
            CategoriesOut categoriesOut = new CategoriesOut();
            registerEventRepository.SaveRegisterEvent(categoriesIn.userId.Value, categoriesIn.key.Value, "Log - Start", "Repository.CategoryRepository.GetCategories", "");

            using (var db = new DBContext())
            {
                categoriesOut.result = db.Categories
                                         .Where(x => x.DeletedDate == null && x.Active == true)
                                         .Select(x => new CategoriesVM()
                                         {
                                             categoryId = x.CategoryId,
                                             code = x.Code,
                                             name = x.Name
                                         }).OrderBy(x => x.code)
                                         .ToList();
            }

            registerEventRepository.SaveRegisterEvent(categoriesIn.userId.Value, categoriesIn.key.Value, "Log - End", "Repository.CategoryRepository.GetCategories", "");
            return categoriesOut;
        }

        private List<string> GetParents(int parentId, List<string> vs)
        {
            var category = (dynamic)null;

            using (var db = new DBContext())
            {
                category = db.Categories.Where(x => x.DeletedDate == null && x.Active == true && x.CategoryId == parentId).Select(x => new { x.Code, x.Name, x.ParentId }).FirstOrDefault();

                vs.Add(category.Code + " - " + category.Name);
            }

            if (category != null && category.ParentId != null)
            {
                GetParents(category.ParentId, vs);
            }

            return vs;
        }
    }
}

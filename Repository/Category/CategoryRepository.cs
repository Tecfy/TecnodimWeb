using ApiTecnodim;
using DataEF.DataAccess;
using Model.In;
using Model.Out;
using Model.VM;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Repository
{
    public class CategoryRepository
    {
        RegisterEventRepository registerEventRepository = new RegisterEventRepository();
        CategoryApi categoryApi = new CategoryApi();

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
                                        name = x.Code + " - " + x.Name,
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

        public CategorySearchOut GetCategorySearch(CategorySearchIn categorySearchIn)
        {
            CategorySearchOut categorySearchOut = new CategorySearchOut();
            registerEventRepository.SaveRegisterEvent(categorySearchIn.userId.Value, categorySearchIn.key.Value, "Log - Start", "Repository.CategoryRepository.GetCategorySearch", "");

            using (var db = new DBContext())
            {
                categorySearchOut.result = db.Categories
                                    .Where(x => x.DeletedDate == null && x.Active == true && x.Code == categorySearchIn.code)
                                    .Select(x => new CategorySearchVM()
                                    {
                                        categoryId = x.CategoryId,
                                        name = x.Code + " - " + x.Name,
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

            if (categorySearchOut.result.parentId != null)
            {
                categorySearchOut.result.parents = GetParents(categorySearchOut.result.parentId.Value, new List<string>());
                categorySearchOut.result.parents = categorySearchOut.result.parents.OrderBy(x => x).ToList();
            }

            registerEventRepository.SaveRegisterEvent(categorySearchIn.userId.Value, categorySearchIn.key.Value, "Log - End", "Repository.CategoryRepository.GetCategorySearch", "");
            return categorySearchOut;
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
                                             name = x.Code + " - " + x.Name
                                         })
                                         .OrderBy(x => x.name)
                                         .ToList();
            }

            registerEventRepository.SaveRegisterEvent(categoriesIn.userId.Value, categoriesIn.key.Value, "Log - End", "Repository.CategoryRepository.GetCategories", "");
            return categoriesOut;
        }

        public ECMCategoriesOut GetECMCategories(ECMCategoriesIn ecmCategoriesIn)
        {
            ECMCategoriesOut ecmCategoriesOut = new ECMCategoriesOut();
            registerEventRepository.SaveRegisterEvent(ecmCategoriesIn.userId.Value, ecmCategoriesIn.key.Value, "Log - Start", "Repository.CategoryRepository.GetECMCategories", "");

            ecmCategoriesOut = categoryApi.GetECMCategories();

            using (var db = new DBContext())
            {
                Categories category = new Categories();

                foreach (var item in ecmCategoriesOut.result)
                {
                    category = new Categories();
                    category = db.Categories.Where(x => x.ExternalId == item.categoryId).FirstOrDefault();

                    if (category == null)
                    {
                        category = new Categories
                        {
                            ExternalId = item.categoryId,
                            Code = item.code,
                            Name = item.name,
                            ParentId = CategorySave(ecmCategoriesOut.result, item.parentId)
                        };

                        db.Categories.Add(category);
                        db.SaveChanges();
                    }
                    else
                    {
                        category.Code = item.code;
                        category.Name = item.name;

                        db.Entry(category).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                    }
                }
            }

            registerEventRepository.SaveRegisterEvent(ecmCategoriesIn.userId.Value, ecmCategoriesIn.key.Value, "Log - End", "Repository.CategoryRepository.GetECMCategories", "");
            return ecmCategoriesOut;
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

        private int? CategorySave(List<ECMCategoriesVM> ecmCategoriesVMs, int parentId)
        {
            int? categoryId = null;

            using (var db = new DBContext())
            {
                Categories category = db.Categories.Where(x => x.ExternalId == parentId).FirstOrDefault();

                if (category == null)
                {
                    ECMCategoriesVM ecmCategoriesVM = ecmCategoriesVMs.Where(x => x.categoryId == parentId).FirstOrDefault();

                    if (ecmCategoriesVM != null)
                    {
                        category = new Categories
                        {
                            ExternalId = ecmCategoriesVM.categoryId,
                            Code = ecmCategoriesVM.code,
                            Name = ecmCategoriesVM.name
                        };

                        if (ecmCategoriesVM.parentId > 0)
                        {
                            category.ParentId = CategorySave(ecmCategoriesVMs, ecmCategoriesVM.parentId);
                        }

                        db.Categories.Add(category);
                        db.SaveChanges();

                        categoryId = category.CategoryId;
                    }
                }
                else
                {
                    categoryId = category.CategoryId;
                }
            }

            return categoryId;
        }
    }
}

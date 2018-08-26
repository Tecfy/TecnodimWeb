using ApiTecnodim;
using DataEF.DataAccess;
using Model.In;
using Model.Out;
using Model.VM;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Repository
{
    public class CategoryRepository
    {
        RegisterEventRepository registerEventRepository = new RegisterEventRepository();
        CategoryApi categoryApi = new CategoryApi();

        #region .: Adm :.

        public bool Delete(int categoryId)
        {
            using (var db = new DBContext())
            {
                Categories category = db.Categories.Find(categoryId);
                category.Active = false;
                category.DeletedDate = DateTime.Now;

                db.Entry(category).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }

            return true;
        }

        public CategoriesOut GetAll(CategoriesIn categoriesIn)
        {
            CategoriesOut categoriesOut = new CategoriesOut();

            using (var db = new DBContext())
            {
                categoriesOut.totalCount = db.Categories.Count(x => x.Active == true && x.DeletedDate == null);

                categoriesOut.result = db.Categories
                                   .Where(x => x.Active == true && x.DeletedDate == null)
                                   .Select(x => new CategoriesVM()
                                   {
                                       CategoryId = x.CategoryId,
                                       Parent = x.Categories1.Name,
                                       Code = x.Code,
                                       Name = x.Name,
                                       CreatedDate = x.CreatedDate
                                   })
                                   .OrderBy(categoriesIn.sort, !categoriesIn.sortdirection.Equals("asc"))
                                   .Skip((categoriesIn.currentPage.Value - 1) * categoriesIn.qtdEntries.Value)
                                   .Take(categoriesIn.qtdEntries.Value)
                                   .ToList();
            }

            return categoriesOut;
        }

        public CategoriesDDLOut GetDDLAll()
        {
            CategoriesDDLOut categoriesDDLOut = new CategoriesDDLOut();

            using (var db = new DBContext())
            {
                categoriesDDLOut.result = db.Categories
                                   .Where(x => x.Active == true && x.DeletedDate == null)
                                   .Select(x => new CategoriesDDLVM()
                                   {
                                       CategoryId = x.CategoryId,
                                       Name = x.Code + " - " + x.Name,
                                   })
                                   .ToList();
            }

            return categoriesDDLOut;
        }

        public CategoryOut GetById(CategoryIn categoryIn)
        {
            CategoryOut categoryOut = new CategoryOut();

            using (var db = new DBContext())
            {
                categoryOut.result = db.Categories
                                   .Where(x => x.Active == true && x.DeletedDate == null && x.CategoryId == categoryIn.CategoryId)
                                   .Select(x => new CategoryVM()
                                   {
                                       CategoryId = x.CategoryId,
                                       Parent = x.Categories1.Name,
                                       Code = x.Code,
                                       Name = x.Name,
                                   }).FirstOrDefault();
            }

            return categoryOut;
        }

        public CategoryEditOut GetEditById(CategoryIn categoryIn)
        {
            CategoryEditOut categoryEditOut = new CategoryEditOut();

            using (var db = new DBContext())
            {
                categoryEditOut.result = db.Categories
                                   .Where(x => x.Active == true && x.DeletedDate == null && x.CategoryId == categoryIn.CategoryId)
                                   .Select(x => new CategoryEditVM()
                                   {
                                       CategoryId = x.CategoryId,
                                       ParentId = x.ParentId,
                                       ExternalId = x.ExternalId,
                                       Code = x.Code,
                                       Name = x.Name,
                                   }).FirstOrDefault();
            }

            return categoryEditOut;
        }

        public CategoryOut Update(CategoryEditIn categoryEditIn)
        {
            CategoryOut categoryOut = new CategoryOut();

            using (var db = new DBContext())
            {
                Categories category = db.Categories.Find(categoryEditIn.CategoryId);

                category.EditedDate = DateTime.Now;
                category.ParentId = categoryEditIn.ParentId;
                category.ExternalId = categoryEditIn.ExternalId;
                category.Code = categoryEditIn.Code;
                category.Name = categoryEditIn.Name;

                db.Entry(category).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                categoryOut.result = db.Categories
                                       .Where(x => x.Active == true && x.DeletedDate == null && x.CategoryId == category.CategoryId)
                                       .Select(x => new CategoryVM()
                                       {
                                           CategoryId = x.CategoryId,
                                           Parent = x.Categories1.Name,
                                           Code = x.Code,
                                           Name = x.Name,
                                       }).FirstOrDefault();
            }

            return categoryOut;
        }

        #endregion

        #region .: Api :.

        public ApiCategoryOut GetCategory(ApiCategoryIn categoryIn)
        {
            ApiCategoryOut categoryOut = new ApiCategoryOut();
            registerEventRepository.SaveRegisterEvent(categoryIn.userId.Value, categoryIn.key.Value, "Log - Start", "Repository.CategoryRepository.GetCategory", "");

            using (var db = new DBContext())
            {
                categoryOut.result = db.Categories
                                    .Where(x => x.DeletedDate == null && x.Active == true && x.CategoryId == categoryIn.categoryId)
                                    .Select(x => new ApiCategoryVM()
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

        public ApiCategorySearchOut GetCategorySearch(ApiCategorySearchIn categorySearchIn)
        {
            ApiCategorySearchOut categorySearchOut = new ApiCategorySearchOut();
            registerEventRepository.SaveRegisterEvent(categorySearchIn.userId.Value, categorySearchIn.key.Value, "Log - Start", "Repository.CategoryRepository.GetCategorySearch", "");

            using (var db = new DBContext())
            {
                categorySearchOut.result = db.Categories
                                    .Where(x => x.DeletedDate == null && x.Active == true && x.Code == categorySearchIn.code)
                                    .Select(x => new ApiCategorySearchVM()
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

        public ApiCategoriesOut GetCategories(ApiCategoriesIn categoriesIn)
        {
            ApiCategoriesOut categoriesOut = new ApiCategoriesOut();
            registerEventRepository.SaveRegisterEvent(categoriesIn.userId.Value, categoriesIn.key.Value, "Log - Start", "Repository.CategoryRepository.GetCategories", "");

            using (var db = new DBContext())
            {
                categoriesOut.result = db.Categories
                                         .Where(x => x.DeletedDate == null && x.Active == true)
                                         .Select(x => new ApiCategoriesVM()
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

        public ApiECMCategoriesOut GetECMCategories(ApiECMCategoriesIn ecmCategoriesIn)
        {
            ApiECMCategoriesOut ecmCategoriesOut = new ApiECMCategoriesOut();
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
                        category.EditedDate = DateTime.Now;

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

        private int? CategorySave(List<ApiECMCategoriesVM> ecmCategoriesVMs, int parentId)
        {
            int? categoryId = null;

            using (var db = new DBContext())
            {
                Categories category = db.Categories.Where(x => x.ExternalId == parentId).FirstOrDefault();

                if (category == null)
                {
                    ApiECMCategoriesVM ecmCategoriesVM = ecmCategoriesVMs.Where(x => x.categoryId == parentId).FirstOrDefault();

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

        #endregion 
    }
}

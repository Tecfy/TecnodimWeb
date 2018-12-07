using DataEF.DataAccess;
using Model.In;
using Model.Out;
using Model.VM;
using System;
using System.Linq;

namespace Repository
{
    public class CategoryAdditionalFieldRepository
    {
        RegisterEventRepository registerEventRepository = new RegisterEventRepository();

        #region .: Adm :.

        public bool Delete(int categoryAdditionalFieldId)
        {
            using (var db = new DBContext())
            {
                CategoryAdditionalFields categoryAdditionalField = db.CategoryAdditionalFields.Find(categoryAdditionalFieldId);
                categoryAdditionalField.Active = false;
                categoryAdditionalField.DeletedDate = DateTime.Now;

                db.Entry(categoryAdditionalField).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }

            return true;
        }

        public CategoryAdditionalFieldEditOut GetEditById(CategoryAdditionalFieldIn categoryAdditionalFieldIn)
        {
            CategoryAdditionalFieldEditOut categoryAdditionalFieldInEditOut = new CategoryAdditionalFieldEditOut();

            using (var db = new DBContext())
            {
                categoryAdditionalFieldInEditOut.result = db.CategoryAdditionalFields
                                                            .Where(x => x.Active == true && x.DeletedDate == null && x.CategoryId == categoryAdditionalFieldIn.CategoryId && x.AdditionalFieldId == categoryAdditionalFieldIn.AdditionalFieldId)
                                                            .Select(x => new CategoryAdditionalFieldEditVM()
                                                            {
                                                                CategoryAdditionalFieldId = x.CategoryAdditionalFieldId,
                                                                CategoryId = x.CategoryId,
                                                                AdditionalFieldId = x.AdditionalFieldId,
                                                                Single = x.Single,
                                                                Required = x.Required,
                                                            }).FirstOrDefault();
            }

            return categoryAdditionalFieldInEditOut;
        }

        public CategoryAdditionalFieldOut Insert(CategoryAdditionalFieldCreateIn categoryAdditionalFieldCreateIn)
        {
            CategoryAdditionalFieldOut categoryAdditionalFieldInOut = new CategoryAdditionalFieldOut();

            using (var db = new DBContext())
            {
                CategoryAdditionalFields categoryAdditionalField = new CategoryAdditionalFields
                {
                    CategoryId = categoryAdditionalFieldCreateIn.CategoryId,
                    AdditionalFieldId = categoryAdditionalFieldCreateIn.AdditionalFieldId,
                    Single = categoryAdditionalFieldCreateIn.Single,
                    Required = categoryAdditionalFieldCreateIn.Required,
                };

                db.CategoryAdditionalFields.Add(categoryAdditionalField);
                db.SaveChanges();

                categoryAdditionalFieldInOut.result = db.CategoryAdditionalFields
                                                        .Where(x => x.Active == true && x.DeletedDate == null && x.CategoryAdditionalFieldId == categoryAdditionalField.CategoryAdditionalFieldId)
                                                        .Select(x => new CategoryAdditionalFieldVM()
                                                        {
                                                            CategoryAdditionalFieldId = x.CategoryAdditionalFieldId,
                                                            CategoryId = x.CategoryId,
                                                            AdditionalFieldId = x.AdditionalFieldId,
                                                            Single = x.Single,
                                                            Required = x.Required,
                                                        }).FirstOrDefault();
            }

            return categoryAdditionalFieldInOut;
        }

        public CategoryAdditionalFieldOut Update(CategoryAdditionalFieldEditIn categoryAdditionalFieldEditIn)
        {
            CategoryAdditionalFieldOut categoryAdditionalFieldInOut = new CategoryAdditionalFieldOut();

            using (var db = new DBContext())
            {
                CategoryAdditionalFields categoryAdditionalField = db.CategoryAdditionalFields.Find(categoryAdditionalFieldEditIn.CategoryAdditionalFieldId);

                categoryAdditionalField.EditedDate = DateTime.Now;
                categoryAdditionalField.CategoryId = categoryAdditionalFieldEditIn.CategoryId;
                categoryAdditionalField.AdditionalFieldId = categoryAdditionalFieldEditIn.AdditionalFieldId;
                categoryAdditionalField.Single = categoryAdditionalFieldEditIn.Single;
                categoryAdditionalField.Required = categoryAdditionalFieldEditIn.Required;

                db.Entry(categoryAdditionalField).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                categoryAdditionalFieldInOut.result = db.CategoryAdditionalFields
                                                        .Where(x => x.Active == true && x.DeletedDate == null && x.CategoryAdditionalFieldId == categoryAdditionalField.CategoryAdditionalFieldId)
                                                        .Select(x => new CategoryAdditionalFieldVM()
                                                        {
                                                            CategoryAdditionalFieldId = x.CategoryAdditionalFieldId,
                                                            CategoryId = x.CategoryId,
                                                            AdditionalFieldId = x.AdditionalFieldId,
                                                            Single = x.Single,
                                                            Required = x.Required,
                                                        }).FirstOrDefault();
            }

            return categoryAdditionalFieldInOut;
        }

        #endregion

        #region .: API :.

        public CategoryAdditionalFieldsOut GetCategoryAdditionalFieldsByCategoryId(CategoryAdditionalFieldsIn categoryAdditionalFieldsIn)
        {
            CategoryAdditionalFieldsOut categoryAdditionalFieldsOut = new CategoryAdditionalFieldsOut();

            using (var db = new DBContext())
            {
                categoryAdditionalFieldsOut.result = db.CategoryAdditionalFields
                                                       .Where(x => x.Active == true
                                                                   && x.DeletedDate == null
                                                                   && x.CategoryId == categoryAdditionalFieldsIn.categoryId)
                                                       .Select(x => new CategoryAdditionalFieldsVM()
                                                       {
                                                           categoryAdditionalFieldId = x.CategoryAdditionalFieldId
                                                       }).ToList();
            }

            return categoryAdditionalFieldsOut;
        }

        #endregion
    }
}

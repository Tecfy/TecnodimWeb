﻿using DataEF.DataAccess;
using Model.In;
using Model.Out;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Repository
{
    public partial class SliceCategoryAdditionalFieldRepository
    {
        private RegisterEventRepository registerEventRepository = new RegisterEventRepository();

        public SliceCategoryAdditionalFieldOut SaveSliceCategoryAdditionalField(SliceCategoryAdditionalFieldIn sliceCategoryAdditionalFieldIn)
        {
            SliceCategoryAdditionalFieldOut sliceCategoryAdditionalFieldOut = new SliceCategoryAdditionalFieldOut();

            registerEventRepository.SaveRegisterEvent(sliceCategoryAdditionalFieldIn.id, sliceCategoryAdditionalFieldIn.key, "Log - Start", "Repository.SliceCategoryAdditionalFieldRepository.SaveSliceCategoryAdditionalField", "");

            using (var db = new DBContext())
            {
                SliceCategoryAdditionalFields sliceCategoryAdditionalField = db.SliceCategoryAdditionalFields
                                                                               .Where(x => x.Active == true 
                                                                                       && x.DeletedDate == null 
                                                                                       && x.SliceId == sliceCategoryAdditionalFieldIn.sliceId 
                                                                                       && x.CategoryAdditionalFieldId == sliceCategoryAdditionalFieldIn.categoryAdditionalFieldId
                                                                                     ).FirstOrDefault();

                if (sliceCategoryAdditionalField == null)
                {
                    sliceCategoryAdditionalField = new SliceCategoryAdditionalFields
                    {
                        SliceId = sliceCategoryAdditionalFieldIn.sliceId,
                        CategoryAdditionalFieldId = sliceCategoryAdditionalFieldIn.categoryAdditionalFieldId,
                        Value = sliceCategoryAdditionalFieldIn.value
                    };

                    db.SliceCategoryAdditionalFields.Add(sliceCategoryAdditionalField);
                    db.SaveChanges();
                }
                else
                {
                    sliceCategoryAdditionalField.EditedDate = DateTime.Now;
                    sliceCategoryAdditionalField.SliceId = sliceCategoryAdditionalFieldIn.sliceId;
                    sliceCategoryAdditionalField.CategoryAdditionalFieldId = sliceCategoryAdditionalFieldIn.categoryAdditionalFieldId;
                    sliceCategoryAdditionalField.Value = sliceCategoryAdditionalFieldIn.value;

                    db.Entry(sliceCategoryAdditionalField).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
            }

            registerEventRepository.SaveRegisterEvent(sliceCategoryAdditionalFieldIn.id, sliceCategoryAdditionalFieldIn.key, "Log - End", "Repository.SliceCategoryAdditionalFieldRepository.SaveSliceCategoryAdditionalField", "");
            return sliceCategoryAdditionalFieldOut;
        }

        public void DeleteSliceCategoryAdditionalField(SliceCategoryAdditionalFieldDeleteIn apiSliceCategoryAdditionalFieldDeleteIn)
        {
            registerEventRepository.SaveRegisterEvent(apiSliceCategoryAdditionalFieldDeleteIn.id, apiSliceCategoryAdditionalFieldDeleteIn.key, "Log - Start", "Repository.SliceCategoryAdditionalFieldRepository.DeleteSliceCategoryAdditionalField", "");

            using (var db = new DBContext())
            {
                List<SliceCategoryAdditionalFields> sliceCategoryAdditionalFields = db.SliceCategoryAdditionalFields
                                                                                      .Where(x => x.Active == true
                                                                                               && x.DeletedDate == null
                                                                                               && x.SliceId == apiSliceCategoryAdditionalFieldDeleteIn.sliceId
                                                                                               && x.CategoryAdditionalFields.CategoryId != apiSliceCategoryAdditionalFieldDeleteIn.categoryId
                                                                                            ).ToList();

                foreach (var item in sliceCategoryAdditionalFields)
                {
                    item.Active = false;
                    item.DeletedDate = DateTime.Now;

                    db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
            }

            registerEventRepository.SaveRegisterEvent(apiSliceCategoryAdditionalFieldDeleteIn.id, apiSliceCategoryAdditionalFieldDeleteIn.key, "Log - End", "Repository.SliceCategoryAdditionalFieldRepository.DeleteSliceCategoryAdditionalField", "");
        }
    }
}

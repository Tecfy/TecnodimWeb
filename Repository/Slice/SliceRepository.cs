using DataEF.DataAccess;
using Model.In;
using Model.Out;
using Model.VM;
using System;
using System.Linq;

namespace Repository
{
    public partial class SliceRepository
    {
        RegisterEventRepository registerEventRepository = new RegisterEventRepository();
        SlicePageRepository slicePageRepository = new SlicePageRepository();

        public SliceOut GetSlice(SliceIn sliceIn)
        {
            SliceOut sliceOut = new SliceOut();
            registerEventRepository.SaveRegisterEvent(sliceIn.userId.Value, sliceIn.key.Value, "Log - Start", "Repository.SliceRepository.GetSlice", "");

            using (var db = new DBContext())
            {
                sliceOut.result = db.Slices
                                    .Where(x => x.Active == true && x.DeletedDate == null && x.SliceId == sliceIn.sliceId)
                                    .Select(x => new SliceVM()
                                    {
                                        sliceId = x.SliceId,
                                        categoryId = x.CategoryId,
                                        name = x.Name,
                                        slicePages = x.SlicePages.Select(y => new SlicePageVM()
                                        {
                                            slicePageId = y.SlicePageId,
                                            page = y.Page,
                                            rotate = y.Rotate,
                                            image = "/Images/GetImage/" + y.Slices.Documents.Hash + "/" + y.Page,
                                            thumb = "/Images/GetImage/" + y.Slices.Documents.Hash + "/" + y.Page + "/true",
                                        }).ToList(),
                                        additionalFields = x.SliceCategoryAdditionalFields.Select(y => new AdditionalFieldVM()
                                        {
                                            categoryAdditionalFieldId = y.CategoryAdditionalFieldId,
                                            name = y.CategoryAdditionalFields.AdditionalFields.Name,
                                            type = y.CategoryAdditionalFields.AdditionalFields.Type,
                                            value = y.Value,
                                            single = y.CategoryAdditionalFields.Single,
                                            required = y.CategoryAdditionalFields.Required,
                                            confidential = y.CategoryAdditionalFields.Confidential,
                                        }).ToList()
                                    })
                                    .FirstOrDefault();
            }

            registerEventRepository.SaveRegisterEvent(sliceIn.userId.Value, sliceIn.key.Value, "Log - End", "Repository.SliceRepository.GetSlice", "");
            return sliceOut;
        }

        public SlicesOut GetSlices(SlicesIn slicesIn)
        {
            SlicesOut slicesOut = new SlicesOut();
            registerEventRepository.SaveRegisterEvent(slicesIn.userId.Value, slicesIn.key.Value, "Log - Start", "Repository.SliceRepository.GetSlices", "");

            using (var db = new DBContext())
            {
                slicesOut.result = db.Slices
                                     .Where(x => x.Active == true && x.DeletedDate == null && x.DocumentId == slicesIn.documentId && (slicesIn.classificated == null || x.CategoryId.HasValue == slicesIn.classificated))
                                     .Select(x => new SlicesVM()
                                     {
                                         sliceId = x.SliceId,
                                         name = x.Name,
                                     })
                                     .OrderBy(x => x.sliceId)
                                     .ToList();
            }

            registerEventRepository.SaveRegisterEvent(slicesIn.userId.Value, slicesIn.key.Value, "Log - End", "Repository.SliceRepository.GetSlices", "");
            return slicesOut;
        }

        public SliceOut SaveSlice(SliceSaveIn sliceIn)
        {
            SliceOut sliceOut = new SliceOut();

            registerEventRepository.SaveRegisterEvent(sliceIn.userId.Value, sliceIn.key.Value, "Log - Start", "Repository.SliceRepository.SaveSlices", "");

            using (var db = new DBContext())
            {
                Documents document = db.Documents.Where(x => x.DocumentId == sliceIn.documentId).FirstOrDefault();

                if (document == null)
                {
                    throw new Exception(i18n.Resource.RegisterNotFound);
                }

                Slices slice = new Slices();
                slice.DocumentId = document.DocumentId;
                slice.Name = sliceIn.name;

                db.Slices.Add(slice);
                db.SaveChanges();

                foreach (var item in sliceIn.pages)
                {
                    SlicePageIn slicePageIn = new SlicePageIn() { key = sliceIn.userId, userId = sliceIn.key, sliceId = slice.SliceId, page = item.page };
                    slicePageRepository.SaveSlicePage(slicePageIn);
                }

                sliceOut.result.sliceId = slice.SliceId;
            }

            registerEventRepository.SaveRegisterEvent(sliceIn.userId.Value, sliceIn.key.Value, "Log - End", "Repository.SliceRepository.SaveSlices", "");
            return sliceOut;
        }

        public SliceOut UpdateSlice(SliceUpdateIn sliceUpdateIn)
        {
            SliceOut sliceOut = new SliceOut();

            registerEventRepository.SaveRegisterEvent(sliceUpdateIn.userId.Value, sliceUpdateIn.key.Value, "Log - Start", "Repository.SliceRepository.UpdateSlice", "");

            using (var db = new DBContext())
            {
                Slices slice = db.Slices.Find(sliceUpdateIn.sliceId);

                if (slice == null)
                {
                    throw new Exception(i18n.Resource.RegisterNotFound);
                }

                slice.EditedDate = DateTime.Now;
                slice.CategoryId = sliceUpdateIn.categoryId;
                slice.Name = sliceUpdateIn.name;

                db.Entry(slice).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }

            registerEventRepository.SaveRegisterEvent(sliceUpdateIn.userId.Value, sliceUpdateIn.key.Value, "Log - End", "Repository.SliceRepository.UpdateSlice", "");
            return sliceOut;
        }
    }
}

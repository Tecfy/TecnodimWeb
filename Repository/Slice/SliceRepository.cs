﻿using DataEF.DataAccess;
using Helper.Enum;
using Model.In;
using Model.Out;
using Model.VM;
using System;
using System.Linq;
using System.Web.Configuration;

namespace Repository
{
    public partial class SliceRepository
    {
        private RegisterEventRepository registerEventRepository = new RegisterEventRepository();
        private SlicePageRepository slicePageRepository = new SlicePageRepository();
        private DocumentRepository documentRepository = new DocumentRepository();
        private PDFRepository pdfRepository = new PDFRepository();

        public SliceOut GetSlice(SliceIn sliceIn)
        {
            SliceOut sliceOut = new SliceOut();
            registerEventRepository.SaveRegisterEvent(sliceIn.id, sliceIn.key, "Log - Start", "Repository.SliceRepository.GetSlice", "");
            string path = WebConfigurationManager.AppSettings["UrlBase"];
            Documents documents = new Documents();
            string now = DateTime.Now.ToString("yyyyMMddHHmmsss");

            using (var db = new DBContext())
            {
                sliceOut.result = db.Slices
                                    .Where(x => x.Active == true && x.DeletedDate == null && x.SliceId == sliceIn.sliceId)
                                    .Select(x => new SliceVM()
                                    {
                                        sliceId = x.SliceId,
                                        documentId = x.DocumentId,
                                        categoryId = x.CategoryId,
                                        name = x.Name,
                                        slicePages = x.SlicePages.Where(y => y.Active == true && y.DeletedDate == null).Select(y => new SlicePageVM()
                                        {
                                            slicePageId = y.SlicePageId,
                                            page = y.Page,
                                            rotate = y.Rotate,
                                            image = path + "/Files/Pages/" + y.Slices.Documents.Hash + "/Images/" + y.Page + ".jpg?" + now,
                                            thumb = path + "/Files/Pages/" + y.Slices.Documents.Hash + "/Thumbs/" + y.Page + ".jpg?" + now,
                                        }).ToList(),
                                        additionalFields = x.SliceCategoryAdditionalFields.Where(y => y.Active == true && y.DeletedDate == null).Select(y => new AdditionalFieldVM()
                                        {
                                            categoryAdditionalFieldId = y.CategoryAdditionalFieldId,
                                            name = y.CategoryAdditionalFields.AdditionalFields.Name,
                                            type = y.CategoryAdditionalFields.AdditionalFields.Type,
                                            value = y.Value,
                                            single = y.CategoryAdditionalFields.Single,
                                            required = y.CategoryAdditionalFields.Required,
                                        }).ToList()
                                    })
                                    .FirstOrDefault();

                documents = db.Documents.Where(x => x.DocumentId == sliceOut.result.documentId).FirstOrDefault();
            }

            if (!documents.Download)
            {
                if (!pdfRepository.ValidPDFs(documents.DocumentId, documents.ExternalId, documents.Hash.ToString(), sliceIn.id, sliceIn.key))
                {
                    throw new Exception(i18n.Resource.FileNotFound);
                }
            }
            else
            {
                throw new Exception(i18n.Resource.DownloadFile);
            }

            registerEventRepository.SaveRegisterEvent(sliceIn.id, sliceIn.key, "Log - End", "Repository.SliceRepository.GetSlice", "");
            return sliceOut;
        }

        public SliceOut GetSlicePending(SlicePendingIn slicePendingIn)
        {
            SliceOut sliceOut = new SliceOut();
            registerEventRepository.SaveRegisterEvent(slicePendingIn.id, slicePendingIn.key, "Log - Start", "Repository.SliceRepository.GetSlicePending", "");
            string path = WebConfigurationManager.AppSettings["UrlBase"];
            Documents documents = new Documents();
            string now = DateTime.Now.ToString("yyyyMMddHHmmsss");

            using (var db = new DBContext())
            {
                sliceOut.result = db.Slices
                                    .Where(x => x.Active == true && x.DeletedDate == null && x.CategoryId == null && x.DocumentId == slicePendingIn.documentId)
                                    .Select(x => new SliceVM()
                                    {
                                        sliceId = x.SliceId,
                                        categoryId = x.CategoryId,
                                        name = x.Name,
                                        slicePages = x.SlicePages.Where(y => y.Active == true && y.DeletedDate == null).Select(y => new SlicePageVM()
                                        {
                                            slicePageId = y.SlicePageId,
                                            page = y.Page,
                                            rotate = y.Rotate,
                                            image = path + "/Files/Pages/" + y.Slices.Documents.Hash + "/Images/" + y.Page + ".jpg?" + now,
                                            thumb = path + "/Files/Pages/" + y.Slices.Documents.Hash + "/Thumbs/" + y.Page + ".jpg?" + now,
                                        }).ToList(),
                                        additionalFields = x.SliceCategoryAdditionalFields.Where(y => y.Active == true && y.DeletedDate == null).Select(y => new AdditionalFieldVM()
                                        {
                                            categoryAdditionalFieldId = y.CategoryAdditionalFieldId,
                                            name = y.CategoryAdditionalFields.AdditionalFields.Name,
                                            type = y.CategoryAdditionalFields.AdditionalFields.Type,
                                            value = y.Value,
                                            single = y.CategoryAdditionalFields.Single,
                                            required = y.CategoryAdditionalFields.Required,
                                        }).ToList()
                                    })
                                    .OrderBy(x => x.sliceId)
                                    .FirstOrDefault();

                if (sliceOut.result == null || sliceOut.result.sliceId <= 0)
                {
                    Documents document = db.Documents.Where(x => x.DocumentId == slicePendingIn.documentId).FirstOrDefault();

                    if (document.DocumentStatusId == (int)EDocumentStatus.PartiallyClassificated)
                    {
                        documentRepository.PostDocumentUpdateSatus(new DocumentUpdateIn { id = slicePendingIn.id, key = slicePendingIn.key, documentId = document.DocumentId, documentStatusId = (int)EDocumentStatus.Classificated });
                    }
                }

                documents = db.Documents.Where(x => x.DocumentId == slicePendingIn.documentId).FirstOrDefault();
            }

            if (!documents.Download)
            {
                if (!pdfRepository.ValidPDFs(documents.DocumentId, documents.ExternalId, documents.Hash.ToString(), slicePendingIn.id, slicePendingIn.key))
                {
                    throw new Exception(i18n.Resource.FileNotFound);
                }
            }
            else
            {
                throw new Exception(i18n.Resource.DownloadFile);
            }

            registerEventRepository.SaveRegisterEvent(slicePendingIn.id, slicePendingIn.key, "Log - End", "Repository.SliceRepository.GetSlicePending", "");
            return sliceOut;
        }

        public SlicesOut GetSlices(SlicesIn slicesIn)
        {
            SlicesOut slicesOut = new SlicesOut();
            registerEventRepository.SaveRegisterEvent(slicesIn.id, slicesIn.key, "Log - Start", "Repository.SliceRepository.GetSlices", "");

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

                if (slicesIn.classificated == false && (slicesOut.result == null || slicesOut.result.Count <= 0))
                {
                    Documents document = db.Documents.Where(x => x.DocumentId == slicesIn.documentId).FirstOrDefault();

                    if (document.DocumentStatusId == (int)EDocumentStatus.PartiallyClassificated)
                    {
                        documentRepository.PostDocumentUpdateSatus(new DocumentUpdateIn { id = slicesIn.id, key = slicesIn.key, documentId = document.DocumentId, documentStatusId = (int)EDocumentStatus.Classificated });
                    }
                }
            }

            registerEventRepository.SaveRegisterEvent(slicesIn.id, slicesIn.key, "Log - End", "Repository.SliceRepository.GetSlices", "");
            return slicesOut;
        }

        public SliceOut SaveSlice(SliceSaveIn sliceIn)
        {
            SliceOut sliceOut = new SliceOut();

            registerEventRepository.SaveRegisterEvent(sliceIn.id, sliceIn.key, "Log - Start", "Repository.SliceRepository.SaveSlices", "");

            using (var db = new DBContext())
            {
                Documents document = db.Documents.Where(x => x.DocumentId == sliceIn.documentId).FirstOrDefault();

                if (document == null)
                {
                    throw new Exception(i18n.Resource.RegisterNotFound);
                }

                if (document.DocumentStatusId == (int)EDocumentStatus.New)
                {
                    documentRepository.PostDocumentUpdateSatus(new DocumentUpdateIn { id = sliceIn.id, key = sliceIn.key, documentId = document.DocumentId, documentStatusId = (int)EDocumentStatus.PartiallySlice });
                }

                int userId = 0;
                userId = db.Users.Where(x => x.AspNetUserId == sliceIn.id).FirstOrDefault().UserId;

                Slices slice = new Slices
                {
                    DocumentId = document.DocumentId,
                    Name = sliceIn.name,
                    SliceUserId = userId,
                    SliceDate = DateTime.Now,
                };

                db.Slices.Add(slice);
                db.SaveChanges();

                foreach (var item in sliceIn.pages)
                {
                    SlicePageIn slicePageIn = new SlicePageIn() { key = sliceIn.id, id = sliceIn.key, sliceId = slice.SliceId, page = item.page };
                    slicePageRepository.SaveSlicePage(slicePageIn);
                }

                sliceOut.result.sliceId = slice.SliceId;
            }

            registerEventRepository.SaveRegisterEvent(sliceIn.id, sliceIn.key, "Log - End", "Repository.SliceRepository.SaveSlices", "");
            return sliceOut;
        }

        public SliceOut MoveSliceNew(SliceMoveNewIn sliceMoveIn)
        {
            SliceOut sliceOut = new SliceOut();
            Slices slice = new Slices();
            Slices sliceOld = new Slices();

            registerEventRepository.SaveRegisterEvent(sliceMoveIn.id, sliceMoveIn.key, "Log - Start", "Repository.SliceRepository.MoveSliceNew", "");

            using (var db = new DBContext())
            {
                sliceOld = db.Slices.FirstOrDefault(x => x.SliceId == sliceMoveIn.sliceId);

                if (sliceOld == null)
                {
                    throw new Exception(i18n.Resource.RegisterNotFound);
                }

                int userId = 0;
                userId = db.Users.Where(x => x.AspNetUserId == sliceMoveIn.id).FirstOrDefault().UserId;

                slice = new Slices
                {
                    DocumentId = sliceOld.DocumentId,
                    Name = sliceMoveIn.name,
                    SliceUserId = userId,
                    SliceDate = DateTime.Now,
                };

                db.Slices.Add(slice);
                db.SaveChanges();
            }

            try
            {
                SlicePageMoveIn slicePageIn = new SlicePageMoveIn()
                {
                    key = sliceMoveIn.id,
                    id = sliceMoveIn.key,
                    sliceNewId = slice.SliceId,
                    sliceOldId = sliceMoveIn.sliceId,
                    page = sliceMoveIn.page
                };

                slicePageRepository.MoveSlicePage(slicePageIn);
            }           
            catch (Exception)
            {
                using (var db = new DBContext())
                {

                    slice.DeletedDate = DateTime.Now;
                    slice.EditedDate = DateTime.Now;
                    slice.Active = false;

                    db.Entry(slice).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
                throw new Exception(i18n.Resource.UnknownError);
            }

            using (var db = new DBContext())
            {
                int countAssets = db.SlicePages.Count(x => x.Active == true && x.DeletedDate == null && x.SliceId == sliceMoveIn.sliceId);

                if (countAssets <= 0)
                {
                    sliceOld.DeletedDate = DateTime.Now;
                    sliceOld.EditedDate = DateTime.Now; ;
                    sliceOld.Active = false;

                    db.Entry(sliceOld).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }

                sliceOut.result.sliceId = slice.SliceId;
            }

            registerEventRepository.SaveRegisterEvent(sliceMoveIn.id, sliceMoveIn.key, "Log - End", "Repository.SliceRepository.MoveSliceNew", "");
            return sliceOut;
        }

        public SliceOut MoveSliceExisting(SliceMoveExistingIn sliceMoveExistingIn)
        {
            SliceOut sliceOut = new SliceOut();

            registerEventRepository.SaveRegisterEvent(sliceMoveExistingIn.id, sliceMoveExistingIn.key, "Log - Start", "Repository.SliceRepository.MoveSliceExisting", "");

            using (var db = new DBContext())
            {
                Slices sliceOld = db.Slices.FirstOrDefault(x => x.SliceId == sliceMoveExistingIn.sliceOldId);

                if (sliceOld == null)
                {
                    throw new Exception(i18n.Resource.RegisterNotFound);
                }

                try
                {
                    SlicePageMoveIn slicePageIn = new SlicePageMoveIn()
                    {
                        key = sliceMoveExistingIn.id,
                        id = sliceMoveExistingIn.key,
                        sliceNewId = sliceMoveExistingIn.sliceNewId,
                        sliceOldId = sliceMoveExistingIn.sliceOldId,
                        page = sliceMoveExistingIn.page
                    };
                    slicePageRepository.MoveSlicePage(slicePageIn);
                }
                catch (Exception)
                {
                    throw new Exception(i18n.Resource.UnknownError);
                }

                int countAssets = db.SlicePages.Count(x => x.Active == true && x.DeletedDate == null && x.SliceId == sliceMoveExistingIn.sliceOldId);

                if (countAssets <= 0)
                {
                    sliceOld.DeletedDate = DateTime.Now;
                    sliceOld.EditedDate = DateTime.Now;
                    sliceOld.Active = false;

                    db.Entry(sliceOld).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }

                sliceOut.result.sliceId = sliceMoveExistingIn.sliceNewId;
            }

            registerEventRepository.SaveRegisterEvent(sliceMoveExistingIn.id, sliceMoveExistingIn.key, "Log - End", "Repository.SliceRepository.MoveSliceExisting", "");
            return sliceOut;
        }

        public SliceOut DeleteSlice(SliceDeleteIn sliceDeleteIn)
        {
            SliceOut sliceOut = new SliceOut();

            registerEventRepository.SaveRegisterEvent(sliceDeleteIn.id, sliceDeleteIn.key, "Log - Start", "Repository.SliceRepository.DeleteSlice", "");

            using (var db = new DBContext())
            {
                Slices slice = db.Slices.FirstOrDefault(x => x.SliceId == sliceDeleteIn.sliceId);

                if (slice == null)
                {
                    throw new Exception(i18n.Resource.RegisterNotFound);
                }

                SlicePageDeleteBySliceIn slicePageDeleteBySliceIn = new SlicePageDeleteBySliceIn()
                {
                    key = sliceDeleteIn.id,
                    id = sliceDeleteIn.key,
                    sliceId = sliceDeleteIn.sliceId
                };

                slicePageRepository.DeleteSlicePageBySlice(slicePageDeleteBySliceIn);

                int countAssets = db.SlicePages.Count(x => x.Active == true && x.DeletedDate == null && x.SliceId == sliceDeleteIn.sliceId);

                if (countAssets <= 0)
                {
                    slice.DeletedDate = DateTime.Now;
                    slice.EditedDate = DateTime.Now;
                    slice.Active = false;

                    db.Entry(slice).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }

                sliceOut.result.sliceId = slice.SliceId;
            }

            registerEventRepository.SaveRegisterEvent(sliceDeleteIn.id, sliceDeleteIn.key, "Log - End", "Repository.SliceRepository.DeleteSlice", "");
            return sliceOut;
        }

        public SliceOut UpdateSlice(SliceUpdateIn sliceUpdateIn)
        {
            SliceOut sliceOut = new SliceOut();

            registerEventRepository.SaveRegisterEvent(sliceUpdateIn.id, sliceUpdateIn.key, "Log - Start", "Repository.SliceRepository.UpdateSlice", "");

            using (var db = new DBContext())
            {
                Slices slice = db.Slices.Find(sliceUpdateIn.sliceId);

                if (slice == null)
                {
                    throw new Exception(i18n.Resource.RegisterNotFound);
                }

                int userId = 0;
                userId = db.Users.Where(x => x.AspNetUserId == sliceUpdateIn.id).FirstOrDefault().UserId;

                slice.EditedDate = DateTime.Now;
                slice.CategoryId = sliceUpdateIn.categoryId;
                slice.ClassificationUserId = userId;
                slice.ClassificationDate = DateTime.Now;

                db.Entry(slice).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                Documents document = db.Documents.Where(x => x.DocumentId == slice.DocumentId).FirstOrDefault();

                if (document.DocumentStatusId == (int)EDocumentStatus.Slice)
                {
                    documentRepository.PostDocumentUpdateSatus(new DocumentUpdateIn { id = sliceUpdateIn.id, key = sliceUpdateIn.key, documentId = document.DocumentId, documentStatusId = (int)EDocumentStatus.PartiallyClassificated });
                }
            }

            registerEventRepository.SaveRegisterEvent(sliceUpdateIn.id, sliceUpdateIn.key, "Log - End", "Repository.SliceRepository.UpdateSlice", "");
            return sliceOut;
        }
    }
}

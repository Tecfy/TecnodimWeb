using DataEF.DataAccess;
using Model.In;
using Model.Out;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Repository
{
    public partial class SlicePageRepository
    {
        private RegisterEventRepository registerEventRepository = new RegisterEventRepository();

        public SlicePageOut SaveSlicePage(SlicePageIn slicePageIn)
        {
            SlicePageOut slicePageOut = new SlicePageOut();

            registerEventRepository.SaveRegisterEvent(slicePageIn.id, slicePageIn.key, "Log - Start", "Repository.SlicePageRepository.SaveSlicePage", "");

            using (var db = new DBContext())
            {
                SlicePages slicePages = new SlicePages();
                slicePages.SliceId = slicePageIn.sliceId;
                slicePages.Page = slicePageIn.page;

                db.SlicePages.Add(slicePages);
                db.SaveChanges();

                slicePageOut.result.slicePageId = slicePages.SlicePageId;
            }

            registerEventRepository.SaveRegisterEvent(slicePageIn.id, slicePageIn.key, "Log - End", "Repository.SlicePageRepository.SaveSlicePage", "");
            return slicePageOut;
        }

        public SlicePageOut MoveSlicePage(SlicePageMoveIn slicePageMoveIn)
        {
            SlicePageOut slicePageOut = new SlicePageOut();

            registerEventRepository.SaveRegisterEvent(slicePageMoveIn.id, slicePageMoveIn.key, "Log - Start", "Repository.SlicePageRepository.MoveSlicePage", "");

            using (var db = new DBContext())
            {
                SlicePages slicePages = db.SlicePages.Where(x => x.Active == true
                                                            && x.DeletedDate == null
                                                            && x.SliceId == slicePageMoveIn.sliceOldId
                                                            && x.Page == slicePageMoveIn.page).FirstOrDefault();

                if (slicePages == null)
                {
                    throw new Exception(i18n.Resource.RegisterNotFound);
                }

                slicePages.SliceId = slicePageMoveIn.sliceNewId;
                slicePages.EditedDate = DateTime.Now;

                db.Entry(slicePages).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                slicePageOut.result.slicePageId = slicePages.SlicePageId;
            }

            registerEventRepository.SaveRegisterEvent(slicePageMoveIn.id, slicePageMoveIn.key, "Log - End", "Repository.SlicePageRepository.MoveSlicePage", "");
            return slicePageOut;
        }

        public void DeleteSlicePage(SlicePageDeleteIn slicePageDeleteIn)
        {
            registerEventRepository.SaveRegisterEvent(slicePageDeleteIn.id, slicePageDeleteIn.key, "Log - Start", "Repository.SlicePageRepository.DeleteSlicePage", "");

            using (var db = new DBContext())
            {
                SlicePages slicePages = db.SlicePages.Find(slicePageDeleteIn.slicePageId);
                slicePages.Active = false;
                slicePages.DeletedDate = DateTime.Now;

                db.Entry(slicePages).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                int countAssets = db.SlicePages.Count(x => x.Active == true && x.DeletedDate == null && x.SliceId == slicePages.SliceId);

                if (countAssets <= 0)
                {
                    Slices slice = db.Slices.FirstOrDefault(x => x.SliceId == slicePages.SliceId);

                    slice.DeletedDate = DateTime.Now;
                    slice.Active = false;

                    db.Entry(slice).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }

            }

            registerEventRepository.SaveRegisterEvent(slicePageDeleteIn.id, slicePageDeleteIn.key, "Log - End", "Repository.SlicePageRepository.DeleteSlicePage", "");
        }

        public void DeleteSlicePageBySlice(SlicePageDeleteBySliceIn slicePageDeleteBySliceIn)
        {
            registerEventRepository.SaveRegisterEvent(slicePageDeleteBySliceIn.id, slicePageDeleteBySliceIn.key, "Log - Start", "Repository.SlicePageRepository.DeleteSlicePageBySlice", "");

            using (var db = new DBContext())
            {
                List<SlicePages> slicePages = db.SlicePages.Where(x => x.Active == true && x.DeletedDate == null && x.SliceId == slicePageDeleteBySliceIn.sliceId).ToList();

                foreach (var slicePage in slicePages)
                {
                    slicePage.Active = false;
                    slicePage.DeletedDate = DateTime.Now;

                    db.Entry(slicePages).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
            }

            registerEventRepository.SaveRegisterEvent(slicePageDeleteBySliceIn.id, slicePageDeleteBySliceIn.key, "Log - End", "Repository.SlicePageRepository.DeleteSlicePageBySlice", "");
        }

        public void UpdateSlicePage(SlicePageUpdateIn slicePageUpdateIn)
        {
            registerEventRepository.SaveRegisterEvent(slicePageUpdateIn.id, slicePageUpdateIn.key, "Log - Start", "Repository.SlicePageRepository.UpdateSlicePage", "");

            using (var db = new DBContext())
            {
                SlicePages slicePages = db.SlicePages.Find(slicePageUpdateIn.slicePageId);

                if (slicePages != null)
                {
                    slicePages.EditedDate = DateTime.Now;
                    slicePages.Rotate = slicePageUpdateIn.rotate;

                    db.Entry(slicePages).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
            }

            registerEventRepository.SaveRegisterEvent(slicePageUpdateIn.id, slicePageUpdateIn.key, "Log - End", "Repository.SlicePageRepository.UpdateSlicePage", "");
        }
    }
}

using DataEF.DataAccess;
using Model.In;
using Model.Out;
using System;

namespace Repository
{
    public partial class SlicePageRepository
    {
        RegisterEventRepository registerEventRepository = new RegisterEventRepository();

        public SlicePageOut SaveSlicePage(SlicePageIn slicePageIn)
        {
            SlicePageOut slicePageOut = new SlicePageOut();

            registerEventRepository.SaveRegisterEvent(slicePageIn.userId.Value, slicePageIn.key.Value, "Log - Start", "Repository.SlicePageRepository.SaveSlicePage", "");

            using (var db = new DBContext())
            {
                SlicePages slicePages = new SlicePages();
                slicePages.SliceId = slicePageIn.sliceId;
                slicePages.Page = slicePageIn.page;

                db.SlicePages.Add(slicePages);
                db.SaveChanges();

                slicePageOut.result.slicePageId = slicePages.SlicePageId;
            }

            registerEventRepository.SaveRegisterEvent(slicePageIn.userId.Value, slicePageIn.key.Value, "Log - End", "Repository.SlicePageRepository.SaveSlicePage", "");
            return slicePageOut;
        }

        public void DeleteSlicePage(SlicePageDeleteIn slicePageDeleteIn)
        {
            registerEventRepository.SaveRegisterEvent(slicePageDeleteIn.userId.Value, slicePageDeleteIn.key.Value, "Log - Start", "Repository.SlicePageRepository.DeleteSlicePage", "");

            using (var db = new DBContext())
            {
                SlicePages slicePages = db.SlicePages.Find(slicePageDeleteIn.slicePageId);
                slicePages.Active = false;
                slicePages.DeletedDate = DateTime.Now;

                db.Entry(slicePages).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }

            registerEventRepository.SaveRegisterEvent(slicePageDeleteIn.userId.Value, slicePageDeleteIn.key.Value, "Log - End", "Repository.SlicePageRepository.DeleteSlicePage", "");
        }

        public void UpdateSlicePage(SlicePageUpdateIn slicePageUpdateIn)
        {
            registerEventRepository.SaveRegisterEvent(slicePageUpdateIn.userId.Value, slicePageUpdateIn.key.Value, "Log - Start", "Repository.SlicePageRepository.UpdateSlicePage", "");

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

            registerEventRepository.SaveRegisterEvent(slicePageUpdateIn.userId.Value, slicePageUpdateIn.key.Value, "Log - End", "Repository.SlicePageRepository.UpdateSlicePage", "");
        }
    }
}

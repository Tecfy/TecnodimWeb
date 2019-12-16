using DataEF.DataAccess;
using Model.In;
using Model.Out;
using System;
using System.Linq;

namespace Repository
{
    public class JobDeletedPageRepository
    {
        private RegisterEventRepository registerEventRepository = new RegisterEventRepository();

        public JobDeletedPageOut SaveJobDeletedPage(JobDeletedPageIn jobDeletedPageIn)
        {
            JobDeletedPageOut jobDeletedPageOut = new JobDeletedPageOut();

            registerEventRepository.SaveRegisterEvent(jobDeletedPageIn.id, jobDeletedPageIn.key, "Log - Start", "Repository.JobDeletedPageRepository.SaveJobDeletedPage", "");

            //using (var db = new DBContext())
            //{
            //    Documents document = db.Documents.Where(x => x.DocumentId == jobDeletedPageIn.documentId).FirstOrDefault();

            //    if (document == null)
            //    {
            //        throw new Exception(i18n.Resource.RegisterNotFound);
            //    }

            //    DeletedPages deletedPage = new DeletedPages();

            //    foreach (var item in jobDeletedPageIn.pages)
            //    {
            //        deletedPage = new DeletedPages();
            //        deletedPage.DocumentId = document.DocumentId;
            //        deletedPage.Page = item.page;

            //        db.DeletedPages.Add(deletedPage);
            //        db.SaveChanges();
            //    }
            //}

            registerEventRepository.SaveRegisterEvent(jobDeletedPageIn.id, jobDeletedPageIn.key, "Log - End", "Repository.JobDeletedPageRepository.SaveJobDeletedPage", "");
            return jobDeletedPageOut;
        }
    }
}

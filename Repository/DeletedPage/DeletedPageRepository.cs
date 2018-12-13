using DataEF.DataAccess;
using Model.In;
using Model.Out;
using System;
using System.Linq;

namespace Repository
{
    public class DeletedPageRepository
    {
        private RegisterEventRepository registerEventRepository = new RegisterEventRepository();

        public DeletedPageOut SaveDeletedPage(DeletedPageIn deletedPageIn)
        {
            DeletedPageOut deletedPageOut = new DeletedPageOut();

            registerEventRepository.SaveRegisterEvent(deletedPageIn.id, deletedPageIn.key, "Log - Start", "Repository.DeletedPageRepository.SaveDeletedPage", "");

            using (var db = new DBContext())
            {
                Documents document = db.Documents.Where(x => x.DocumentId == deletedPageIn.documentId).FirstOrDefault();

                if (document == null)
                {
                    throw new Exception(i18n.Resource.RegisterNotFound);
                }

                DeletedPages deletedPage = new DeletedPages();

                foreach (var item in deletedPageIn.pages)
                {
                    deletedPage = new DeletedPages();
                    deletedPage.DocumentId = document.DocumentId;
                    deletedPage.Page = item.page;

                    db.DeletedPages.Add(deletedPage);
                    db.SaveChanges();
                }
            }

            registerEventRepository.SaveRegisterEvent(deletedPageIn.id, deletedPageIn.key, "Log - End", "Repository.DeletedPageRepository.SaveDeletedPage", "");
            return deletedPageOut;
        }
    }
}

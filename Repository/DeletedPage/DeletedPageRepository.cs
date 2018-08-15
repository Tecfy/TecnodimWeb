using DataEF.DataAccess;
using Model.In;
using Model.Out;
using System.Linq;

namespace Repository
{
    public class DeletedPageRepository
    {
        RegisterEventRepository registerEventRepository = new RegisterEventRepository();

        public DeletedPageOut SaveDeletedPage(DeletedPageIn deletedPageIn)
        {
            DeletedPageOut deletedPageOut = new DeletedPageOut();

            registerEventRepository.SaveRegisterEvent(deletedPageIn.userId.Value, deletedPageIn.key.Value, "Log - Start", "Repository.DeletedPageRepository.SaveDeletedPage", "");

            using (var db = new DBContext())
            {
                Documents document = db.Documents.Where(x => x.ExternalId == deletedPageIn.externalId).FirstOrDefault();

                if (document == null)
                {
                    document = new Documents();
                    document.ExternalId = deletedPageIn.externalId;

                    db.Documents.Add(document);
                    db.SaveChanges();
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

            registerEventRepository.SaveRegisterEvent(deletedPageIn.userId.Value, deletedPageIn.key.Value, "Log - End", "Repository.DeletedPageRepository.SaveDeletedPage", "");
            return deletedPageOut;
        }
    }
}

using DataEF.DataAccess;
using Model.In;
using Model.Out;
using Model.VM;
using System;
using System.Linq;

namespace Repository
{
    public partial class ClippingRepository
    {
        RegisterEventRepository registerEventRepository = new RegisterEventRepository();
        ClippingPageRepository clippingPageRepository = new ClippingPageRepository();

        #region .: Methods :.

        public ClippingsOut GetClippings(ClippingsIn clippingsIn)
        {
            ClippingsOut clippingsOut = new ClippingsOut();
            registerEventRepository.SaveRegisterEvent(clippingsIn.userId.Value, clippingsIn.key.Value, "Log - Start", "Repository.ClippingRepository.GetClippings", "");

            using (var db = new DBContext())
            {
                clippingsOut.result = db.Clippings
                                        .Where(x => x.Active == true && x.DeletedDate == null && x.DocumentId == clippingsIn.documentId)
                                        .Select(x => new ClippingsVM()
                                        {
                                            clippingId = x.ClippingId,
                                            name = x.Name,
                                            clippingPages = x.ClippingPages.Select(y => new ClippingPageVM()
                                            {
                                                clippingPageId = y.ClippingPageId,
                                                page = y.Page,
                                                Rotate = y.Rotate,
                                                image = "/Images?documentId=" + y.Clippings.Documents.ExternalId + "&pageId=" + y.Page,
                                                thumb = "/Images?documentId=" + y.Clippings.Documents.ExternalId + "&pageId=" + y.Page + "&thumb=true",
                                            }).ToList()
                                        })
                                        .OrderBy(x => x.clippingId)
                                        .ToList();
            }

            registerEventRepository.SaveRegisterEvent(clippingsIn.userId.Value, clippingsIn.key.Value, "Log - End", "Repository.ClippingRepository.GetClippings", "");
            return clippingsOut;
        }

        public ClippingOut SaveClipping(ClippingIn clippingIn)
        {
            ClippingOut clippingOut = new ClippingOut();

            registerEventRepository.SaveRegisterEvent(clippingIn.userId.Value, clippingIn.key.Value, "Log - Start", "Repository.ClippingRepository.SaveClippings", "");

            using (var db = new DBContext())
            {
                Documents document = db.Documents.Where(x => x.ExternalId == clippingIn.documentId).FirstOrDefault();

                if (document == null)
                {
                    document = new Documents();
                    document.ExternalId = clippingIn.documentId;

                    db.Documents.Add(document);
                    db.SaveChanges();
                }

                Clippings clipping = new Clippings();
                clipping.DocumentId = document.DocumentId;
                clipping.Name = clippingIn.name;

                db.Clippings.Add(clipping);
                db.SaveChanges();

                foreach (var item in clippingIn.pages)
                {
                    ClippingPageIn clippingPageIn = new ClippingPageIn() { key = clippingIn.userId, userId = clippingIn.key, clippingId = clipping.ClippingId, page = item.pageId };
                    clippingPageRepository.SaveClippingPage(clippingPageIn);
                }

                clippingOut.result.clippingId = clipping.ClippingId;
            }

            registerEventRepository.SaveRegisterEvent(clippingIn.userId.Value, clippingIn.key.Value, "Log - End", "Repository.ClippingRepository.SaveClippings", "");
            return clippingOut;
        }

        #endregion
    }
}

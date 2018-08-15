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

        public ClippingOut GetClipping(ClippingIn clippingIn)
        {
            ClippingOut clippingOut = new ClippingOut();
            registerEventRepository.SaveRegisterEvent(clippingIn.userId.Value, clippingIn.key.Value, "Log - Start", "Repository.ClippingRepository.GetClipping", "");

            using (var db = new DBContext())
            {
                clippingOut.result = db.Clippings
                                        .Where(x => x.Active == true && x.DeletedDate == null && x.ClippingId == clippingIn.clippingId)
                                        .Select(x => new ClippingVM()
                                        {
                                            clippingId = x.ClippingId,
                                            categoryId = x.CategoryId,
                                            name = x.Name,
                                            classification = x.Classification,
                                            clippingPages = x.ClippingPages.Select(y => new ClippingPageVM()
                                            {
                                                clippingPageId = y.ClippingPageId,
                                                page = y.Page,
                                                rotate = y.Rotate,
                                                image = "/Images?externalId=" + y.Clippings.Documents.ExternalId + "&page=" + y.Page,
                                                thumb = "/Images?externalId=" + y.Clippings.Documents.ExternalId + "&page=" + y.Page + "&thumb=true",
                                            }).ToList()
                                        })
                                        .FirstOrDefault();
            }

            registerEventRepository.SaveRegisterEvent(clippingIn.userId.Value, clippingIn.key.Value, "Log - End", "Repository.ClippingRepository.GetClipping", "");
            return clippingOut;
        }

        public ClippingsOut GetClippings(ClippingsIn clippingsIn)
        {
            ClippingsOut clippingsOut = new ClippingsOut();
            registerEventRepository.SaveRegisterEvent(clippingsIn.userId.Value, clippingsIn.key.Value, "Log - Start", "Repository.ClippingRepository.GetClippings", "");

            using (var db = new DBContext())
            {
                clippingsOut.result = db.Clippings
                                        .Where(x => x.Active == true && x.DeletedDate == null && x.Documents.ExternalId == clippingsIn.externalId)
                                        .Select(x => new ClippingsVM()
                                        {
                                            clippingId = x.ClippingId,
                                            name = x.Name,
                                        })
                                        .OrderBy(x => x.clippingId)
                                        .ToList();
            }

            registerEventRepository.SaveRegisterEvent(clippingsIn.userId.Value, clippingsIn.key.Value, "Log - End", "Repository.ClippingRepository.GetClippings", "");
            return clippingsOut;
        }

        public ClippingOut SaveClipping(ClippingSaveIn clippingIn)
        {
            ClippingOut clippingOut = new ClippingOut();

            registerEventRepository.SaveRegisterEvent(clippingIn.userId.Value, clippingIn.key.Value, "Log - Start", "Repository.ClippingRepository.SaveClippings", "");

            using (var db = new DBContext())
            {
                Documents document = db.Documents.Where(x => x.ExternalId == clippingIn.externalId).FirstOrDefault();

                if (document == null)
                {
                    document = new Documents();
                    document.ExternalId = clippingIn.externalId;

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
                    ClippingPageIn clippingPageIn = new ClippingPageIn() { key = clippingIn.userId, userId = clippingIn.key, clippingId = clipping.ClippingId, page = item.page };
                    clippingPageRepository.SaveClippingPage(clippingPageIn);
                }

                clippingOut.result.clippingId = clipping.ClippingId;
            }

            registerEventRepository.SaveRegisterEvent(clippingIn.userId.Value, clippingIn.key.Value, "Log - End", "Repository.ClippingRepository.SaveClippings", "");
            return clippingOut;
        }

        public ClippingOut UpdateClipping(ClippingUpdateIn clippingUpdateIn)
        {
            ClippingOut clippingOut = new ClippingOut();

            registerEventRepository.SaveRegisterEvent(clippingUpdateIn.userId.Value, clippingUpdateIn.key.Value, "Log - Start", "Repository.ClippingRepository.UpdateClipping", "");

            using (var db = new DBContext())
            {
                Clippings clipping = db.Clippings.Find(clippingUpdateIn.clippingId);

                if (clipping == null)
                {
                    throw new Exception(i18n.Resource.RegisterNotFound);
                }

                clipping.CategoryId = clippingUpdateIn.categoryId;
                clipping.Name = clippingUpdateIn.name;
                clipping.Classification = clippingUpdateIn.classification;

                db.Entry(clipping).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }

            registerEventRepository.SaveRegisterEvent(clippingUpdateIn.userId.Value, clippingUpdateIn.key.Value, "Log - End", "Repository.ClippingRepository.UpdateClipping", "");
            return clippingOut;
        }
    }
}

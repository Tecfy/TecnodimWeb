using ApiTecnodim;
using DataEF.DataAccess;
using Helper;
using Helper.Enum;
using Model.In;
using Model.Out;
using Model.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Configuration;
using WebSupergoo.ABCpdf11;

namespace Repository
{
    public partial class DocumentRepository
    {
        private RegisterEventRepository registerEventRepository = new RegisterEventRepository();
        private UnityRepository unityRepository = new UnityRepository();
        private AttributeApi attributeApi = new AttributeApi();
        private DocumentApi documentApi = new DocumentApi();

        #region .: API :.

        #region .: ECM :.

        public ECMDocumentOut GetECMDocumentById(DocumentIn documentIn)
        {
            ECMDocumentOut ecmDocumentOut = new ECMDocumentOut();
            registerEventRepository.SaveRegisterEvent(documentIn.id, documentIn.key, "Log - Start", "Repository.DocumentRepository.GetDocumentById", "");

            string externalId = string.Empty;
            Guid hash = new Guid();

            using (var db = new DBContext())
            {
                Documents document = db.Documents.Where(x => x.DocumentId == documentIn.documentId).FirstOrDefault();

                if (document == null)
                {
                    throw new Exception(i18n.Resource.RegisterNotFound);
                }

                externalId = document.ExternalId;
                hash = document.Hash;
            }

            ecmDocumentOut = documentApi.GetECMDocument(externalId);
            ecmDocumentOut.result.hash = hash.ToString();

            registerEventRepository.SaveRegisterEvent(documentIn.id, documentIn.key, "Log - End", "Repository.DocumentRepository.GetDocumentById", "");
            return ecmDocumentOut;
        }

        public ECMDocumentOut GetECMDocumentByHash(string hash)
        {
            ECMDocumentOut ecmDocumentOut = new ECMDocumentOut();
            Guid guid = Guid.Parse(hash);
            string externalId = string.Empty;

            using (var db = new DBContext())
            {
                externalId = db.Documents.Where(x => x.Hash == guid).FirstOrDefault().ExternalId;
            }

            ecmDocumentOut = documentApi.GetECMDocument(externalId);
            ecmDocumentOut.result.hash = hash;

            return ecmDocumentOut;
        }

        public ECMDocumentsOut GetECMDocuments(ECMDocumentsIn ecmDocumentsIn)
        {
            ECMDocumentsOut ecmDocumentsOut = new ECMDocumentsOut();
            registerEventRepository.SaveRegisterEvent(ecmDocumentsIn.id, ecmDocumentsIn.key, "Log - Start", "Repository.DocumentRepository.GetECMDocuments", "");

            ecmDocumentsOut = documentApi.GetECMDocuments();

            if (ecmDocumentsOut.result != null && ecmDocumentsOut.result.Count > 0)
            {
                using (var db = new DBContext())
                {
                    Documents document = new Documents();

                    foreach (var item in ecmDocumentsOut.result)
                    {
                        document = new Documents();
                        document = db.Documents.Where(x => x.ExternalId == item.externalId).FirstOrDefault();

                        int? unityId = unityRepository.GetByCode(item.unity);
                        if (unityId != null && unityId > 0)
                        {
                            if (document == null)
                            {
                                document = new Documents
                                {
                                    ExternalId = item.externalId,
                                    DocumentStatusId = item.documentStatusId,
                                    UnityId = unityId.Value,
                                    Registration = item.registration,
                                    Name = item.name
                                };

                                db.Documents.Add(document);
                                db.SaveChanges();

                                List<ECMAttributeItemIn> itens = new List<ECMAttributeItemIn>
                                {
                                    new ECMAttributeItemIn { attribute = WebConfigurationManager.AppSettings["Repository.DocumentRepository.Attribute"].ToString(), value = WebConfigurationManager.AppSettings["Repository.DocumentRepository.Slice"].ToString() }
                                };
                                attributeApi.PostECMAttributeUpdate(new ECMAttributeIn(item.externalId, itens));
                            }
                            else
                            {
                                List<ECMAttributeItemIn> itens = new List<ECMAttributeItemIn>
                                {
                                    new ECMAttributeItemIn { attribute = WebConfigurationManager.AppSettings["Repository.DocumentRepository.Attribute"].ToString(), value = WebConfigurationManager.AppSettings["Repository.DocumentRepository.Slice"].ToString() }
                                };
                                attributeApi.PostECMAttributeUpdate(new ECMAttributeIn(document.ExternalId, itens));
                            }
                        }
                        else
                        {
                            registerEventRepository.SaveRegisterEvent(ecmDocumentsIn.id, ecmDocumentsIn.key, "Erro", "Repository.DocumentRepository.GetECMDocuments", string.Format(i18n.Resource.UnityNotFound, item.unity));
                        }
                    }
                }
            }

            registerEventRepository.SaveRegisterEvent(ecmDocumentsIn.id, ecmDocumentsIn.key, "Log - End", "Repository.DocumentRepository.GetECMDocuments", "");
            return ecmDocumentsOut;
        }

        public ECMDocumentsSendOut GetECMSendDocuments(ECMDocumentsSendIn ecmDocumentsSendIn)
        {
            ECMDocumentsSendOut ecmDocumentsSendOut = new ECMDocumentsSendOut();
            registerEventRepository.SaveRegisterEvent(ecmDocumentsSendIn.id, ecmDocumentsSendIn.key, "Log - Start", "Repository.DocumentRepository.GetECMSendDocuments", "");

            #region .: Search Documents Finished :.

            DocumentsFinishedOut documentsFinishedOut = GetDocumentsFinished(new DocumentsFinishedIn() { id = ecmDocumentsSendIn.id, key = ecmDocumentsSendIn.key });

            #endregion

            #region .: Process Queue :.

            foreach (var item in documentsFinishedOut.result)
            {
                try
                {
                    DocumentSliceProcess(item, ecmDocumentsSendIn.id, ecmDocumentsSendIn.key);
                }
                catch (Exception ex)
                {
                    registerEventRepository.SaveRegisterEvent(ecmDocumentsSendIn.id, ecmDocumentsSendIn.key, "Erro", "Repository.DocumentRepository.GetECMSendDocuments", ex.Message);

                    ecmDocumentsSendOut.messages.Add(ex.Message);
                }
            }

            #endregion

            #region .: Search Documents Sent :.

            DocumentsSentOut documentsSentOut = GetDocumentsSent(new DocumentsSentIn() { id = ecmDocumentsSendIn.id, key = ecmDocumentsSendIn.key });

            #endregion

            #region .: Update Documents :.

            foreach (var item in documentsSentOut.result)
            {
                string externalId;

                using (var db = new DBContext())
                {
                    Documents document = db.Documents.Find(item.documentId);
                    externalId = document.ExternalId;

                    document.DocumentStatusId = (int)EDocumentStatus.Sent;
                    document.EditedDate = DateTime.Now;

                    db.Entry(document).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }

                try
                {
                    ECMDocumentDeletedIn ecmDocumentDeletedIn = new ECMDocumentDeletedIn() { externalId = externalId, id = ecmDocumentsSendIn.id, key = ecmDocumentsSendIn.key };

                    documentApi.DeleteECMDocumentArchive(ecmDocumentDeletedIn);

                    List<ECMAttributeItemIn> itens = new List<ECMAttributeItemIn>
                    {
                        new ECMAttributeItemIn { attribute = WebConfigurationManager.AppSettings["Repository.DocumentRepository.Attribute"].ToString(), value = WebConfigurationManager.AppSettings["Repository.DocumentRepository.Finished"].ToString() },
                    };

                    attributeApi.PostECMAttributeUpdate(new ECMAttributeIn(externalId, itens));
                }
                catch { }
            }

            #endregion

            registerEventRepository.SaveRegisterEvent(ecmDocumentsSendIn.id, ecmDocumentsSendIn.key, "Log - End", "Repository.DocumentRepository.GetECMSendDocuments", "");
            return ecmDocumentsSendOut;
        }

        public ECMDocumentsValidateOut GetECMValidateDocuments(ECMDocumentsValidateIn ecmDocumentsValidateIn)
        {
            ECMDocumentsValidateOut ecmDocumentsValidateOut = new ECMDocumentsValidateOut();
            registerEventRepository.SaveRegisterEvent(ecmDocumentsValidateIn.id, ecmDocumentsValidateIn.key, "Log - Start", "Repository.DocumentRepository.GetECMValidateDocuments", "");

            ecmDocumentsValidateOut = documentApi.GetECMValidateDocuments();

            if (ecmDocumentsValidateOut.result != null && ecmDocumentsValidateOut.result.Count > 0)
            {
                using (var db = new DBContext())
                {
                    Documents document = new Documents();

                    foreach (var item in ecmDocumentsValidateOut.result)
                    {
                        document = new Documents();
                        document = db.Documents.Where(x => x.ExternalId == item.externalId).FirstOrDefault();

                        if (document != null)
                        {
                            if (document.DocumentStatusId != (int)EDocumentStatus.Sent)
                            {
                                document.DocumentStatusId = (int)EDocumentStatus.Canceled;

                                db.Entry(document).State = System.Data.Entity.EntityState.Modified;
                                db.SaveChanges();
                            }
                        }
                    }
                }
            }

            registerEventRepository.SaveRegisterEvent(ecmDocumentsValidateIn.id, ecmDocumentsValidateIn.key, "Log - End", "Repository.DocumentRepository.GetECMValidateDocuments", "");
            return ecmDocumentsValidateOut;
        }
        
        public ECMDocumentsValidateAdInterfaceOut GetECMValidateAdInterfaceDocuments(ECMDocumentsValidateAdInterfaceIn eCMDocumentsValidateAdInterfaceIn)
        {
            ECMDocumentsValidateAdInterfaceOut eCMDocumentsValidateAdInterfaceOut = new ECMDocumentsValidateAdInterfaceOut();
            registerEventRepository.SaveRegisterEvent(eCMDocumentsValidateAdInterfaceIn.id, eCMDocumentsValidateAdInterfaceIn.key, "Log - Start", "Repository.DocumentRepository.GetECMValidateAdInterfaceDocuments", "");

            eCMDocumentsValidateAdInterfaceOut = documentApi.GetECMValidateAdInterfaceDocuments();

            registerEventRepository.SaveRegisterEvent(eCMDocumentsValidateAdInterfaceIn.id, eCMDocumentsValidateAdInterfaceIn.key, "Log - End", "Repository.DocumentRepository.GetECMValidateAdInterfaceDocuments", "");
            return eCMDocumentsValidateAdInterfaceOut;
        }

        #endregion

        #region .: Local :.

        public DocumentsFinishedOut GetDocumentsFinished(DocumentsFinishedIn documentsFinishedIn)
        {
            DocumentsFinishedOut documentsFinishedOut = new DocumentsFinishedOut();
            registerEventRepository.SaveRegisterEvent(documentsFinishedIn.id, documentsFinishedIn.key, "Log - Start", "Repository.DocumentRepository.GetDocumentsFinished", "");

            #region .: Documents Finished :.

            using (var db = new DBContext())
            {
                documentsFinishedOut.result = db.Slices
                                               .Where(x => x.Active == true
                                                        && x.DeletedDate == null
                                                        && x.Sent == false
                                                        && x.Sending == false
                                                        && x.Documents.Active == true
                                                        && x.Documents.DeletedDate == null
                                                        && x.Documents.DocumentStatusId == (int)EDocumentStatus.Finished)
                                               .Select(x => new DocumentsFinishedVM()
                                               {
                                                   registration = x.Documents.Registration,
                                                   categoryId = x.Categories.Code,
                                                   documentId = x.DocumentId,
                                                   sliceId = x.SliceId,
                                                   externalId = x.Documents.ExternalId,
                                                   title = x.Categories.Name,
                                                   pb = x.Categories.Pb,
                                                   user = x.Users1 != null ? x.Users1.Registration : "",
                                                   sliceUser = x.Users1 != null ? x.Users1.FirstName + " " + x.Users1.LastName : "",
                                                   sliceUserRegistration = x.Users1 != null ? x.Users1.Registration : "",
                                                   classificationUser = x.Users != null ? x.Users.FirstName + " " + x.Users.LastName : "",
                                                   classificationUserRegistration = x.Users != null ? x.Users.Registration : "",
                                                   extension = ".pdf",
                                                   classificationDate = x.ClassificationDate.Value,
                                                   sliceDate = x.SliceDate.Value,
                                                   pages = x.SlicePages
                                                            .Where(y => y.Active == true && y.DeletedDate == null)
                                                            .Select(y => new SlicePagesFinishedVM()
                                                            {
                                                                slicePageId = y.SlicePageId,
                                                                page = y.Page,
                                                                rotate = y.Rotate,
                                                            }).ToList(),
                                                   additionalFields = x.SliceCategoryAdditionalFields
                                                                       .Where(y => y.Active == true && y.DeletedDate == null)
                                                                       .Select(y => new AdditionalFieldSaveVM()
                                                                       {
                                                                           additionalFieldId = y.CategoryAdditionalFields.AdditionalFieldId,
                                                                           value = y.Value,
                                                                       }).ToList()
                                               })
                                               .ToList();
            }

            #endregion

            registerEventRepository.SaveRegisterEvent(documentsFinishedIn.id, documentsFinishedIn.key, "Log - End", "Repository.DocumentRepository.GetDocumentsFinished", "");
            return documentsFinishedOut;
        }

        public DocumentsSentOut GetDocumentsSent(DocumentsSentIn documentsFinishedIn)
        {
            DocumentsSentOut documentsSentOut = new DocumentsSentOut();
            registerEventRepository.SaveRegisterEvent(documentsFinishedIn.id, documentsFinishedIn.key, "Log - Start", "Repository.DocumentRepository.GetDocumentsSent", "");

            #region .: Documents Finished :.

            using (var db = new DBContext())
            {
                documentsSentOut.result = db.Documents
                                            .Where(x => x.Active == true
                                                    && x.DeletedDate == null
                                                    && x.DocumentStatusId == (int)EDocumentStatus.Finished
                                                    && (x.Slices.Count(y => y.Active == true && y.DeletedDate == null) == x.Slices.Count(y => y.Active == true && y.DeletedDate == null && y.Sent == true)))
                                            .Select(x => new DocumentsSentVM()
                                            {
                                                documentId = x.DocumentId,
                                            })
                                            .ToList();
            }

            #endregion

            registerEventRepository.SaveRegisterEvent(documentsFinishedIn.id, documentsFinishedIn.key, "Log - End", "Repository.DocumentRepository.GetDocumentsSent", "");
            return documentsSentOut;
        }

        public DocumentsOut GetDocuments(DocumentsIn documentsIn)
        {
            DocumentsOut documentsOut = new DocumentsOut();
            registerEventRepository.SaveRegisterEvent(documentsIn.id, documentsIn.key, "Log - Start", "Repository.DocumentRepository.GetDocuments", "");

            using (var db = new DBContext())
            {
                var query = db.Documents
                              .Where(x => x.Active == true
                                          && x.DeletedDate == null
                                          && documentsIn.documentStatusIds.Contains(x.DocumentStatusId)
                                          && x.UnityId == documentsIn.unityId
                                          && (documentsIn.registration == null || x.Registration.Contains(documentsIn.registration))
                                          && (documentsIn.name == null || x.Name.Contains(documentsIn.name))
                                          && (documentsIn.documentStatusId == 0 || x.DocumentStatusId == documentsIn.documentStatusId));

                documentsOut.totalCount = query.Count();

                documentsOut.result = query
                                      .Select(x => new DocumentsVM()
                                      {
                                          documentId = x.DocumentId,
                                          name = x.Name,
                                          registration = x.Registration,
                                          statusId = x.DocumentStatusId,
                                          status = x.DocumentStatus.Name,
                                          CreatedDate = x.CreatedDate,
                                      })
                                      .OrderBy(documentsIn.sort, !documentsIn.sortdirection.Equals("asc"))
                                      .Skip((documentsIn.currentPage.Value - 1) * documentsIn.qtdEntries.Value)
                                      .Take(documentsIn.qtdEntries.Value)
                                      .ToList();
            }

            registerEventRepository.SaveRegisterEvent(documentsIn.id, documentsIn.key, "Log - End", "Repository.DocumentRepository.GetDocuments", "");
            return documentsOut;
        }

        public DocumentUpdateOut PostDocumentUpdateSatus(DocumentUpdateIn documentUpdateIn)
        {
            DocumentUpdateOut documentUpdateOut = new DocumentUpdateOut();
            registerEventRepository.SaveRegisterEvent(documentUpdateIn.id, documentUpdateIn.key, "Log - Start", "Repository.DocumentRepository.PostDocumentUpdateSatus", "");

            using (var db = new DBContext())
            {
                Documents document = db.Documents.Where(x => x.Active == true && x.DeletedDate == null && x.DocumentId == documentUpdateIn.documentId).FirstOrDefault();

                if (document == null)
                {
                    throw new Exception(i18n.Resource.RegisterNotFound);
                }

                document.DocumentStatusId = documentUpdateIn.documentStatusId;
                document.EditedDate = DateTime.Now;

                if (documentUpdateIn.documentStatusId == (int)EDocumentStatus.Slice)
                {
                    document.SliceDate = DateTime.Now;
                }
                else if (documentUpdateIn.documentStatusId == (int)EDocumentStatus.Classificated)
                {
                    document.ClassificationDate = DateTime.Now;
                }

                db.Entry(document).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                Users user = db.Users.Where(x => x.AspNetUserId == documentUpdateIn.id).FirstOrDefault();

                if (documentUpdateIn.documentStatusId == (int)EDocumentStatus.Slice)
                {
                    List<ECMAttributeItemIn> itens = new List<ECMAttributeItemIn>
                    {
                        new ECMAttributeItemIn { attribute = WebConfigurationManager.AppSettings["Repository.DocumentRepository.Attribute"].ToString(), value = WebConfigurationManager.AppSettings["Repository.DocumentRepository.Classification"].ToString() },
                        new ECMAttributeItemIn { attribute = WebConfigurationManager.AppSettings["Repository.DocumentRepository.Attribute.SliceDate"].ToString(), value = document.SliceDate.Value.ToString("yyyy-MM-dd") },
                        new ECMAttributeItemIn { attribute = WebConfigurationManager.AppSettings["Repository.DocumentRepository.Attribute.SliceTime"].ToString(), value = document.SliceDate.Value.ToString("HH:mm") },
                        new ECMAttributeItemIn { attribute = WebConfigurationManager.AppSettings["Repository.DocumentRepository.Attribute.SliceUserRegistration"].ToString(), value = user.Registration },
                        new ECMAttributeItemIn { attribute = WebConfigurationManager.AppSettings["Repository.DocumentRepository.Attribute.SliceUser"].ToString(), value = user.FirstName + " " + user.LastName }
                    };

                    attributeApi.PostECMAttributeUpdate(new ECMAttributeIn(document.ExternalId, itens));
                }
                else if (documentUpdateIn.documentStatusId == (int)EDocumentStatus.Classificated)
                {
                    List<ECMAttributeItemIn> itens = new List<ECMAttributeItemIn>
                    {
                        new ECMAttributeItemIn { attribute = WebConfigurationManager.AppSettings["Repository.DocumentRepository.Attribute"].ToString(), value = WebConfigurationManager.AppSettings["Repository.DocumentRepository.Classified"].ToString() },
                        new ECMAttributeItemIn { attribute = WebConfigurationManager.AppSettings["Repository.DocumentRepository.Attribute.ClassificationDate"].ToString(), value = document.ClassificationDate.Value.ToString("yyyy-MM-dd") },
                        new ECMAttributeItemIn { attribute = WebConfigurationManager.AppSettings["Repository.DocumentRepository.Attribute.ClassificationTime"].ToString(), value = document.ClassificationDate.Value.ToString("HH:mm") },
                        new ECMAttributeItemIn { attribute = WebConfigurationManager.AppSettings["Repository.DocumentRepository.Attribute.ClassificationUserRegistration"].ToString(), value = user.Registration },
                        new ECMAttributeItemIn { attribute = WebConfigurationManager.AppSettings["Repository.DocumentRepository.Attribute.ClassificationUser"].ToString(), value = user.FirstName + " " + user.LastName }
                    };

                    attributeApi.PostECMAttributeUpdate(new ECMAttributeIn(document.ExternalId, itens));
                }
            }

            registerEventRepository.SaveRegisterEvent(documentUpdateIn.id, documentUpdateIn.key, "Log - End", "Repository.DocumentRepository.PostDocumentUpdateSatus", "");
            return documentUpdateOut;
        }

        #endregion

        #endregion

        #region .: Helper :.

        public List<int> GetRemainingDocumentPages(RemainingDocumenPagestIn remainingDocumenPagestIn)
        {
            List<int> pages = new List<int>();
            registerEventRepository.SaveRegisterEvent(remainingDocumenPagestIn.id, remainingDocumenPagestIn.key, "Log - Start", "Repository.DocumentRepository.GetRemainingDocumentPages", "");

            using (var db = new DBContext())
            {
                pages.AddRange(db.SlicePages.Where(x => x.Active == true && x.DeletedDate == null && x.Slices.DocumentId == remainingDocumenPagestIn.documentId).Select(x => x.Page).ToList());
                pages.AddRange(db.DeletedPages.Where(x => x.Active == true && x.DeletedDate == null && x.DocumentId == remainingDocumenPagestIn.documentId).Select(x => x.Page).ToList());
            }

            registerEventRepository.SaveRegisterEvent(remainingDocumenPagestIn.id, remainingDocumenPagestIn.key, "Log - End", "Repository.DocumentRepository.GetRemainingDocumentPages", "");
            return pages;
        }

        private void DocumentSliceProcess(DocumentsFinishedVM documentsFinishedVM, string id, string key)
        {
            registerEventRepository.SaveRegisterEvent(id, key, "Log - Start", "Repository.DocumentRepository.DocumentSliceProcess", "");

            Slices slice = new Slices();

            try
            {
                #region .: Validate Slice :.

                using (var db = new DBContext())
                {
                    slice = db.Slices.Where(x => x.Sent == false && x.Sending == false && x.SliceId == documentsFinishedVM.sliceId).FirstOrDefault();

                    if (slice == null)
                    {
                        throw new Exception(string.Format(i18n.Resource.SliceNoProcess, documentsFinishedVM.sliceId));
                    }

                    slice.Sending = true;
                    slice.SendingDate = DateTime.Now;

                    db.Entry(slice).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }

                #endregion

                #region .: Search Document Original :.

                ECMDocumentOut ecmDocumentOut = documentApi.GetECMDocument(documentsFinishedVM.externalId);

                if (!ecmDocumentOut.success)
                {
                    throw new Exception(ecmDocumentOut.messages.FirstOrDefault());
                }

                #endregion

                #region .: Slice Document :.

                PDFIn pdfIn = new PDFIn
                {
                    archive = ecmDocumentOut.result.archive,
                    pb = documentsFinishedVM.pb,
                    pages = documentsFinishedVM.pages,
                };

                string file = Rotate(pdfIn, id, key);

                if (string.IsNullOrEmpty(file))
                {
                    throw new Exception(i18n.Resource.FileNotFound);
                }

                #endregion

                #region .: Sent New Document :.

                List<AdditionalFieldSaveIn> additionalFieldSaveIns = new List<AdditionalFieldSaveIn>();

                if (documentsFinishedVM.additionalFields != null && documentsFinishedVM.additionalFields.Count() > 0)
                {
                    foreach (var item in documentsFinishedVM.additionalFields)
                    {
                        additionalFieldSaveIns.Add(new AdditionalFieldSaveIn { additionalFieldId = item.additionalFieldId, value = item.value });
                    }
                }

                ECMDocumentSaveIn ecmDocumentSaveIn = new ECMDocumentSaveIn
                {
                    registration = documentsFinishedVM.registration,
                    categoryId = documentsFinishedVM.categoryId,
                    archive = file,
                    title = documentsFinishedVM.title,
                    user = documentsFinishedVM.user,
                    sliceUser = documentsFinishedVM.sliceUser,
                    sliceUserRegistration = documentsFinishedVM.sliceUserRegistration,
                    classificationUser = documentsFinishedVM.classificationUser,
                    classificationUserRegistration = documentsFinishedVM.classificationUserRegistration,
                    extension = documentsFinishedVM.extension,
                    classificationDate = documentsFinishedVM.classificationDate.ToString(),
                    sliceDate = documentsFinishedVM.sliceDate.ToString(),
                    additionalFields = additionalFieldSaveIns
                };

                ECMDocumentSaveOut ecmDocumentSaveOut = documentApi.PostECMDocumentSave(ecmDocumentSaveIn);

                if (!ecmDocumentSaveOut.success)
                {
                    throw new Exception(ecmDocumentOut.messages.FirstOrDefault());
                }

                #endregion

                #region .: Update Slice :.

                using (var db = new DBContext())
                {
                    slice.Sent = true;
                    slice.Sending = false;

                    db.Entry(slice).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }

                #endregion
            }
            catch (Exception ex)
            {
                if (slice != null)
                {
                    using (var db = new DBContext())
                    {
                        slice.Sending = false;

                        db.Entry(slice).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                    }
                }

                throw new Exception(ex.Message);
            }

            registerEventRepository.SaveRegisterEvent(id, key, "Log - End", "Repository.DocumentRepository.DocumentSliceProcess", "");
        }

        public string Rotate(PDFIn pdfIn, string id, string key)
        {
            registerEventRepository.SaveRegisterEvent(id, key, "Log - Start", "Repository.DocumentRepository.DocumentSliceProcess", "");

            string archive = string.Empty;

            Doc docOld = new Doc();
            Doc docNew = new Doc();
            docOld.Read(Convert.FromBase64String(pdfIn.archive));

            int theCount = docOld.PageCount;
            string thePages = String.Join(",", pdfIn.pages.Select(x => x.page).ToList());
            docOld.RemapPages(thePages);

            for (int p = 1; p <= docOld.PageCount; p++)
            {
                docOld.PageNumber = p;

                if (pdfIn.pages[p - 1].rotate != null && pdfIn.pages[p - 1].rotate > 0)
                    if (pdfIn.pages[p - 1].rotate % 90 == 0)
                        docOld.SetInfo(docOld.Page, "/Rotate", pdfIn.pages[p - 1].rotate.ToString());
            }

            if (pdfIn.pb)
            {
                docNew = PB.Converter(docOld);

                archive = System.Convert.ToBase64String(docNew.GetData());
            }
            else
            {
                archive = System.Convert.ToBase64String(docOld.GetData());
            }

            docOld.Clear();
            docNew.Clear();

            registerEventRepository.SaveRegisterEvent(id, key, "Log - End", "Repository.DocumentRepository.Rotate", "");

            return archive;
        }

        #endregion
    }
}

using ApiTecnodim;
using DataEF.DataAccess;
using Helper;
using Helper.Enum;
using Helper.ServerMap;
using Model.In;
using Model.Out;
using Model.VM;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
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

        public DocumentVM Documents { get; private set; }

        #region .: API :.

        #region .: ECM :.

        public void GetECMDocument(DocumentIn documentIn, string pathFile, int exec)
        {
            Documents documents = new Documents();
            registerEventRepository.SaveRegisterEvent(documentIn.id, documentIn.key, "Log - Start", "Repository.DocumentRepository.GetECMDocument", "");

            using (var db = new DBContext())
            {
                documents = db.Documents.Where(x => x.DocumentId == documentIn.documentId).FirstOrDefault();

                if (documents == null)
                {
                    registerEventRepository.SaveRegisterEvent(documentIn.id, documentIn.key, "Erro", "Repository.DocumentRepository.GetECMDocument", string.Format("DocumentId: {0}", documentIn.documentId));

                    throw new Exception(i18n.Resource.RegisterNotFound);
                }

                documents.Download = true;
                documents.DownloadDate = DateTime.Now;

                db.Entry(documents).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }

            if (!File.Exists(pathFile))
            {
                try
                {
                    ECMDocumentOut eCMDocumentOut = documentApi.GetECMDocument(documents.ExternalId);

                    if (!eCMDocumentOut.success)
                    {
                        registerEventRepository.SaveRegisterEvent(documentIn.id, documentIn.key, "Erro", "Repository.DocumentRepository.GetECMDocument", string.Format("ExternalId: {0}. Message: {1}", documents.ExternalId, eCMDocumentOut.messages.FirstOrDefault()));

                        throw new Exception(eCMDocumentOut.messages.FirstOrDefault());
                    }
                }
                catch
                {
                    using (var db = new DBContext())
                    {
                        documents.Download = false;

                        db.Entry(documents).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                    }

                    throw new Exception(i18n.Resource.UnknownError);
                }
            }

            if (File.Exists(pathFile))
            {
                try
                {
                    Doc theDoc = new Doc();
                    theDoc.Read(pathFile);

                    string path = ServerMapHelper.GetServerMap(WebConfigurationManager.AppSettings["Path.Files"]);
                    string pathImages = Path.Combine(path, "Pages", documents.Hash.ToString(), "Images");
                    string pathThumb = Path.Combine(path, "Pages", documents.Hash.ToString(), "Thumbs");
                    int dpi = 100;
                    int.TryParse(WebConfigurationManager.AppSettings["DPI"], out dpi);

                    if (!Directory.Exists(pathImages))
                    {
                        Directory.CreateDirectory(pathImages);
                    }

                    if (!Directory.Exists(pathThumb))
                    {
                        Directory.CreateDirectory(pathThumb);
                    }

                    documents.Pages = theDoc.PageCount;

                    for (int i = 1; i <= theDoc.PageCount; i++)
                    {
                        theDoc.PageNumber = i;
                        theDoc.Rect.Resize(theDoc.MediaBox.Width, theDoc.MediaBox.Height);
                        theDoc.Rendering.DotsPerInch = dpi;
                        theDoc.Rendering.GetBitmap().Save(Path.Combine(pathImages, i + ".jpg"));
                        var bmp = theDoc.Rendering.GetBitmap();

                        HelperImage.Resize((Image)bmp, Path.Combine(pathThumb, i + ".jpg"), 200, HelperImage.TypeSize.Width, 342, 80, false, HelperImage.TypeImage.JPG, Color.White);
                    }

                    theDoc.Clear();

                    using (var db = new DBContext())
                    {
                        documents.Download = false;

                        db.Entry(documents).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    using (var db = new DBContext())
                    {
                        documents.Download = false;

                        db.Entry(documents).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                    }
                    registerEventRepository.SaveRegisterEvent(documentIn.id, documentIn.key, "Erro", "Repository.DocumentRepository.GetECMDocument", string.Format("Source: {0}.\n InnerException: {1}.\n Message: {2}", ex.Source, ex.InnerException, ex.Message));

                    throw new Exception(i18n.Resource.UnknownError);
                }
            }
            else
            {
                using (var db = new DBContext())
                {
                    documents.Download = false;

                    db.Entry(documents).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }

                registerEventRepository.SaveRegisterEvent(documentIn.id, documentIn.key, "Erro", "Repository.DocumentRepository.GetECMDocument", string.Format("Erro: {0} PathFile: {1}", i18n.Resource.FileNotFound, pathFile));

                if (exec < 5)
                {
                    exec++;
                    int sleep = 3000;
                    int.TryParse(ConfigurationManager.AppSettings["SLEEP"], out sleep);

                    Thread.Sleep(sleep);
                    GetECMDocument(documentIn, pathFile, exec);
                }
                else
                {
                    throw new Exception(i18n.Resource.FileNotFound);
                }
            }

            registerEventRepository.SaveRegisterEvent(documentIn.id, documentIn.key, "Log - End", "Repository.DocumentRepository.GetECMDocument", "");
        }

        public DocumentOut GetECMDocumentById(DocumentIn documentIn)
        {
            DocumentOut documentOut = new DocumentOut();
            registerEventRepository.SaveRegisterEvent(documentIn.id, documentIn.key, "Log - Start", "Repository.DocumentRepository.GetDocumentById", "");

            using (var db = new DBContext())
            {
                documentOut.result = db.Documents
                                          .Where(x => x.DocumentId == documentIn.documentId)
                                          .Select(x => new DocumentVM()
                                          {
                                              DocumentId = x.DocumentId,
                                              ExternalId = x.ExternalId,
                                              Hash = x.Hash,
                                              Pages = x.Pages,
                                              Download = x.Download
                                          })
                                          .FirstOrDefault();

                if (documentOut.result == null)
                {
                    throw new Exception(i18n.Resource.RegisterNotFound);
                }
            }

            registerEventRepository.SaveRegisterEvent(documentIn.id, documentIn.key, "Log - End", "Repository.DocumentRepository.GetDocumentById", "");
            return documentOut;
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
                        try
                        {
                            if (!string.IsNullOrEmpty(item.name) && !string.IsNullOrEmpty(item.registration) && !string.IsNullOrEmpty(item.externalId))
                            {
                                document = new Documents();
                                document = db.Documents.Where(x => x.ExternalId == item.externalId).FirstOrDefault();

                                int? unityId = unityRepository.GetByCode(item.unityCode, item.unity);
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
                                        if (!document.Active && document.DeletedDate != null)
                                        {
                                            document.DocumentStatusId = (int)EDocumentStatus.New;
                                            document.Active = true;
                                            document.CreatedDate = DateTime.Now;
                                            document.EditedDate = DateTime.Now;
                                            document.DeletedDate = null;
                                            document.SliceDate = null;
                                            document.ClassificationDate = null;
                                            document.Pages = null;
                                            document.Download = false;
                                            document.DownloadDate = null;

                                            db.Entry(document).State = System.Data.Entity.EntityState.Modified;
                                            db.SaveChanges();
                                        }
                                        try
                                        {
                                            List<ECMAttributeItemIn> itens = new List<ECMAttributeItemIn>
                                        {
                                            new ECMAttributeItemIn { attribute = WebConfigurationManager.AppSettings["Repository.DocumentRepository.Attribute"].ToString(), value = WebConfigurationManager.AppSettings["Repository.DocumentRepository.Slice"].ToString() }
                                        };

                                            attributeApi.PostECMAttributeUpdate(new ECMAttributeIn(document.ExternalId, itens));
                                        }
                                        catch (Exception ex)
                                        {
                                            registerEventRepository.SaveRegisterEvent(ecmDocumentsIn.id, ecmDocumentsIn.key, "Erro", "Repository.DocumentRepository.GetECMDocuments", string.Format("ExternalId: {0}, Erro: {1}", document.ExternalId, ex.Message));
                                        }
                                    }
                                }
                                else
                                {
                                    registerEventRepository.SaveRegisterEvent(ecmDocumentsIn.id, ecmDocumentsIn.key, "Erro", "Repository.DocumentRepository.GetECMDocuments", string.Format(i18n.Resource.UnityNotFound, item.unityCode));
                                }
                            }
                            else
                            {
                                registerEventRepository.SaveRegisterEvent(ecmDocumentsIn.id, ecmDocumentsIn.key, "Erro", "Repository.DocumentRepository.GetECMDocuments", string.Format("Parametro em branco. Name: {0}, Registration: {1}, ExternalId: {2}", item.name, item.registration, item.externalId));
                            }
                        }
                        catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
                        {
                            foreach (var validationErrors in dbEx.EntityValidationErrors)
                            {
                                foreach (var validationError in validationErrors.ValidationErrors)
                                {
                                    registerEventRepository.SaveRegisterEvent(ecmDocumentsIn.id, ecmDocumentsIn.key, "Erro", "Repository.DocumentRepository.GetECMDocuments", string.Format("Erro SQL ExternalId: {0}, Property: {1} Error: {2}", item.externalId, validationError.PropertyName, validationError.ErrorMessage));
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            registerEventRepository.SaveRegisterEvent(ecmDocumentsIn.id, ecmDocumentsIn.key, "Erro", "Repository.DocumentRepository.GetECMDocuments", string.Format("Erro Desconhecido: ExternalId: {0}, Erro: {1}", item.externalId, ex.Message));
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

            Documents document = new Documents();
            string path = ServerMapHelper.GetServerMap(WebConfigurationManager.AppSettings["Path.Files"]);
            string pathImages = Path.Combine(path, "Pages", "{0}");
            string pathFile = Path.Combine(path, "Documents", "{0}.pdf");

            foreach (var item in documentsSentOut.result)
            {
                document = new Documents();

                using (var db = new DBContext())
                {
                    document = db.Documents.Find(item.documentId);

                    document.DocumentStatusId = (int)EDocumentStatus.Sent;
                    document.EditedDate = DateTime.Now;

                    db.Entry(document).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }

                try
                {
                    if (File.Exists(string.Format(pathFile, document.ExternalId)))
                    {
                        File.Delete(string.Format(pathFile, document.ExternalId));
                    }

                    if (Directory.Exists(string.Format(pathImages, document.Hash)))
                    {
                        Directory.Delete(string.Format(pathImages, document.Hash), true);
                    }

                    List<ECMAttributeItemIn> itens = new List<ECMAttributeItemIn>
                    {
                        new ECMAttributeItemIn { attribute = WebConfigurationManager.AppSettings["Repository.DocumentRepository.Attribute"].ToString(), value = WebConfigurationManager.AppSettings["Repository.DocumentRepository.Finished"].ToString() },
                    };

                    attributeApi.PostECMAttributeUpdate(new ECMAttributeIn(document.ExternalId, itens));
                }
                catch { }
            }

            #endregion

            registerEventRepository.SaveRegisterEvent(ecmDocumentsSendIn.id, ecmDocumentsSendIn.key, "Log - End", "Repository.DocumentRepository.GetECMSendDocuments", "");
            return ecmDocumentsSendOut;
        }

        public void ConvertDocumentPB(string document)
        {
            Doc docOld = new Doc();
            Doc docNew = new Doc();
            docOld.Read(document);

            docNew = PB.Converter(docOld);

            FileInfo fileInfo = new FileInfo(document);

            docNew.Save(Path.Combine(fileInfo.DirectoryName, fileInfo.Name.Substring(0, fileInfo.Name.LastIndexOf(fileInfo.Extension)) + "_" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_sss") + fileInfo.Extension));

            docOld.Clear();
            docNew.Clear();
        }

        public DocumentValidateSliceOut GetDocumentValidateSlice(DocumentValidateSliceIn documentValidateSliceIn)
        {
            DocumentValidateSliceOut documentValidateSliceOut = new DocumentValidateSliceOut();
            registerEventRepository.SaveRegisterEvent(documentValidateSliceIn.id, documentValidateSliceIn.key, "Log - Start", "Repository.DocumentRepository.GetDocumentValidateSlice", "");

            using (var db = new DBContext())
            {
                Documents document = db.Documents.Where(x => x.DocumentId == documentValidateSliceIn.documentId).FirstOrDefault();

                if (document == null)
                {
                    throw new Exception(i18n.Resource.RegisterNotFound);
                }

                int time = 10;
                int.TryParse(WebConfigurationManager.AppSettings["Dossier.Security.Time"], out time);

                if (document.SliceStart != null)
                {
                    Users userOwner = db.Users.Where(x => x.UserId == document.SliceUser).FirstOrDefault();

                    if (userOwner.AspNetUserId != documentValidateSliceIn.id)
                    {
                        if (document.SliceStart.Value.AddMinutes(time) > DateTime.Now)
                        {
                            throw new Exception(string.Format(i18n.Resource.DocumentValidateMessage, userOwner.FirstName + " " + userOwner.LastName));
                        }
                    }
                }

                Users user = db.Users.Where(x => x.AspNetUserId == documentValidateSliceIn.id).FirstOrDefault();

                document.SliceUser = user.UserId;
                document.SliceStart = DateTime.Now;
                document.EditedDate = DateTime.Now;

                db.Entry(document).State = EntityState.Modified;
                db.SaveChanges();
            }

            registerEventRepository.SaveRegisterEvent(documentValidateSliceIn.id, documentValidateSliceIn.key, "Log - End", "Repository.DocumentRepository.GetDocumentValidateSlice", "");
            return documentValidateSliceOut;
        }

        public DocumentValidateClassificationOut GetDocumentValidateClassification(DocumentValidateClassificationIn documentValidateClassificationIn)
        {
            DocumentValidateClassificationOut documentValidateClassificationOut = new DocumentValidateClassificationOut();
            registerEventRepository.SaveRegisterEvent(documentValidateClassificationIn.id, documentValidateClassificationIn.key, "Log - Start", "Repository.DocumentRepository.GetDocumentValidateClassification", "");

            using (var db = new DBContext())
            {
                Documents document = db.Documents.Where(x => x.DocumentId == documentValidateClassificationIn.documentId).FirstOrDefault();

                if (document == null)
                {
                    throw new Exception(i18n.Resource.RegisterNotFound);
                }

                int time = 10;
                int.TryParse(WebConfigurationManager.AppSettings["Dossier.Security.Time"], out time);

                if (document.ClassificationStart != null)
                {
                    Users userOwner = db.Users.Where(x => x.UserId == document.ClassificationUser).FirstOrDefault();

                    if (userOwner.AspNetUserId != documentValidateClassificationIn.id)
                    {
                        if (document.ClassificationStart.Value.AddMinutes(time) > DateTime.Now)
                        {
                            throw new Exception(string.Format(i18n.Resource.DocumentValidateMessage, userOwner.FirstName + " " + userOwner.LastName));
                        }
                    }
                }

                Users user = db.Users.Where(x => x.AspNetUserId == documentValidateClassificationIn.id).FirstOrDefault();

                document.ClassificationUser = user.UserId;
                document.ClassificationStart = DateTime.Now;
                document.EditedDate = DateTime.Now;

                db.Entry(document).State = EntityState.Modified;
                db.SaveChanges();
            }

            registerEventRepository.SaveRegisterEvent(documentValidateClassificationIn.id, documentValidateClassificationIn.key, "Log - End", "Repository.DocumentRepository.GetDocumentValidateClassification", "");
            return documentValidateClassificationOut;
        }

        #endregion

        #region .: Local :.

        public DocumentsFinishedOut GetDocumentsFinished(DocumentsFinishedIn documentsFinishedIn)
        {
            DocumentsFinishedOut documentsFinishedOut = new DocumentsFinishedOut();
            registerEventRepository.SaveRegisterEvent(documentsFinishedIn.id, documentsFinishedIn.key, "Log - Start", "Repository.DocumentRepository.GetDocumentsFinished", "");
            DateTime yesterday = DateTime.Now.AddDays(-1);

            #region .: Documents Finished :.

            using (var db = new DBContext())
            {
                documentsFinishedOut.result = db.Slices
                                               .Where(x => x.Active == true
                                                        && x.DeletedDate == null
                                                        && x.Sent == false
                                                        && (x.Sending == false || (x.Sending == true && x.SendingDate < yesterday))
                                                        && (x.Processing == false || (x.Processing == true && x.ProcessingDate < yesterday))
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
                                                   unityCode = x.Documents.Units.ExternalId,
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

                foreach (var item in documentsFinishedOut.result)
                {

                    Slices slice = db.Slices.Where(x => x.SliceId == item.sliceId).FirstOrDefault();

                    slice.Sent = false;
                    slice.Sending = false;
                    slice.SendingDate = null;
                    slice.Processing = true;
                    slice.ProcessingDate = DateTime.Now;

                    db.Entry(slice).State = EntityState.Modified;
                    db.SaveChanges();
                }
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
                                          && (documentsIn.documentStatusId == 0 || x.DocumentStatusId == documentsIn.documentStatusId)
                                          && (string.IsNullOrEmpty(documentsIn.externalId) || x.ExternalId == documentsIn.externalId)
                                          && (documentsIn.documentId == 0 || x.DocumentId == documentsIn.documentId));

                documentsOut.totalCount = query.Count();

                documentsOut.result = query
                                      .Select(x => new DocumentsVM()
                                      {
                                          documentId = x.DocumentId,
                                          name = x.Name,
                                          registration = x.Registration,
                                          statusId = x.DocumentStatusId,
                                          status = x.DocumentStatus.Name,
                                          externalId = x.ExternalId,
                                          createdDate = x.CreatedDate,
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
                        registerEventRepository.SaveRegisterEvent(id, key, "Erro", "Repository.DocumentRepository.DocumentSliceProcess", "Slice");

                        throw new Exception(string.Format(i18n.Resource.SliceNoProcess, documentsFinishedVM.sliceId));
                    }

                    slice.Sending = true;
                    slice.SendingDate = DateTime.Now;

                    db.Entry(slice).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }

                #endregion

                #region .: Search Document Original :.

                string path = ServerMapHelper.GetServerMap(WebConfigurationManager.AppSettings["Path.Files"]);
                string name = documentsFinishedVM.externalId + ".pdf";
                string pathFile = Path.Combine(path, "Documents", name);

                if (!File.Exists(pathFile))
                {
                    ECMDocumentOut eCMDocumentOut = documentApi.GetECMDocument(documentsFinishedVM.externalId);

                    if (!eCMDocumentOut.success || !File.Exists(pathFile))
                    {
                        if (!eCMDocumentOut.success)
                        {
                            registerEventRepository.SaveRegisterEvent(id, key, "Erro", "Repository.DocumentRepository.DocumentSliceProcess", "Document");

                            throw new Exception(eCMDocumentOut.messages.FirstOrDefault());
                        }
                        else
                        {
                            registerEventRepository.SaveRegisterEvent(id, key, "Erro", "Repository.DocumentRepository.DocumentSliceProcess", "File Not Found");

                            throw new Exception(i18n.Resource.FileNotFound);
                        }
                    }
                }

                #endregion

                #region .: Slice Document :.

                PDFIn pdfIn = new PDFIn
                {
                    archive = pathFile,
                    pb = documentsFinishedVM.pb,
                    pages = documentsFinishedVM.pages,
                };

                string file = Rotate(pdfIn, id, key);

                if (string.IsNullOrEmpty(file))
                {
                    registerEventRepository.SaveRegisterEvent(id, key, "Erro", "Repository.DocumentRepository.DocumentSliceProcess", "Rotate File");

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
                    title = documentsFinishedVM.title,
                    user = documentsFinishedVM.user,
                    sliceUser = documentsFinishedVM.sliceUser,
                    sliceUserRegistration = documentsFinishedVM.sliceUserRegistration,
                    classificationUser = documentsFinishedVM.classificationUser,
                    classificationUserRegistration = documentsFinishedVM.classificationUserRegistration,
                    extension = documentsFinishedVM.extension,
                    classificationDate = documentsFinishedVM.classificationDate.ToString(),
                    sliceDate = documentsFinishedVM.sliceDate.ToString(),
                    additionalFields = additionalFieldSaveIns,
                    archive = file,
                    unityCode = documentsFinishedVM.unityCode,
                    externalId = documentsFinishedVM.externalId
                };

                try
                {
                    ECMDocumentSaveOut ecmDocumentSaveOut = documentApi.PostECMDocumentSave(ecmDocumentSaveIn);

                    if (!ecmDocumentSaveOut.success)
                    {
                        throw new Exception(ecmDocumentSaveOut.messages.FirstOrDefault());
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
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
                        slice.Processing = false;

                        db.Entry(slice).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                    }
                }

                registerEventRepository.SaveRegisterEvent(id, key, "Erro", "Repository.DocumentRepository.DocumentSliceProcess", string.Format("Source: {0}.\n InnerException: {1}.\n Message: {2}", ex.Source, ex.InnerException, ex.Message));

                throw new Exception(ex.Message);
            }

            registerEventRepository.SaveRegisterEvent(id, key, "Log - End", "Repository.DocumentRepository.DocumentSliceProcess", "");
        }

        private string Rotate(PDFIn pdfIn, string id, string key)
        {
            registerEventRepository.SaveRegisterEvent(id, key, "Log - Start", "Repository.DocumentRepository.Rotate", "");

            string archive = string.Empty;

            Doc docOld = new Doc();
            Doc docNew = new Doc();
            docOld.Read(pdfIn.archive);

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

                archive = Convert.ToBase64String(docNew.GetData());
            }
            else
            {
                archive = Convert.ToBase64String(docOld.GetData());
            }

            docOld.Clear();
            docNew.Clear();

            registerEventRepository.SaveRegisterEvent(id, key, "Log - End", "Repository.DocumentRepository.Rotate", "");

            return archive;
        }

        #endregion
    }
}

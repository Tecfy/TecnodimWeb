using ApiTecnodim;
using DataEF.DataAccess;
using Model.In;
using Model.Out;
using Model.VM;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Repository
{
    public partial class DocumentRepository
    {
        RegisterEventRepository registerEventRepository = new RegisterEventRepository();
        DocumentApi documentApi = new DocumentApi();

        public ECMDocumentOut GetECMDocumentById(DocumentIn documentIn)
        {
            ECMDocumentOut ecmDocumentOut = new ECMDocumentOut();
            registerEventRepository.SaveRegisterEvent(documentIn.userId.Value, documentIn.key.Value, "Log - Start", "Repository.DocumentRepository.GetDocumentById", "");

            string externalId = string.Empty;
            Guid hash = new Guid();

            using (var db = new DBContext())
            {
                Documents document = db.Documents.Where(x => x.DocumentId == documentIn.documentId).FirstOrDefault();

                externalId = document.ExternalId;
                hash = document.Hash;
            }

            ecmDocumentOut = documentApi.GetECMDocument(externalId);
            ecmDocumentOut.result.hash = hash.ToString();

            registerEventRepository.SaveRegisterEvent(documentIn.userId.Value, documentIn.key.Value, "Log - End", "Repository.DocumentRepository.GetDocumentById", "");
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
            registerEventRepository.SaveRegisterEvent(ecmDocumentsIn.userId.Value, ecmDocumentsIn.key.Value, "Log - Start", "Repository.DocumentRepository.GetECMDocuments", "");

            ecmDocumentsOut = documentApi.GetECMDocuments();

            using (var db = new DBContext())
            {
                Documents document = new Documents();

                foreach (var item in ecmDocumentsOut.result)
                {
                    document = new Documents();
                    document = db.Documents.Where(x => x.ExternalId == item.externalId).FirstOrDefault();

                    if (document != null)
                    {
                        document = new Documents
                        {
                            ExternalId = item.externalId,
                            DocumentStatusId = item.documentStatusId,
                            Registration = item.registration,
                            Name = item.name
                        };

                        db.Documents.Add(document);
                        db.SaveChanges();
                    }
                }
            }

            registerEventRepository.SaveRegisterEvent(ecmDocumentsIn.userId.Value, ecmDocumentsIn.key.Value, "Log - End", "Repository.DocumentRepository.GetECMDocuments", "");
            return ecmDocumentsOut;
        }

        public DocumentsOut GetDocuments(DocumentsIn documentsIn)
        {
            DocumentsOut documentsOut = new DocumentsOut();
            registerEventRepository.SaveRegisterEvent(documentsIn.userId.Value, documentsIn.key.Value, "Log - Start", "Repository.DocumentRepository.GetDocuments", "");

            using (var db = new DBContext())
            {
                documentsOut.result = db.Documents
                                        .Where(x => x.Active == true && x.DeletedDate == null && documentsIn.documentStatusIds.Contains(x.DocumentStatusId))
                                        .Select(x => new DocumentsVM()
                                        {
                                            documentId = x.DocumentId,
                                            name = x.Name,
                                            registration = x.Registration,
                                            status = x.DocumentStatus.Name,
                                        }).ToList();
            }

            registerEventRepository.SaveRegisterEvent(documentsIn.userId.Value, documentsIn.key.Value, "Log - End", "Repository.DocumentRepository.GetDocuments", "");
            return documentsOut;
        }

        public List<int> GetRemainingDocumentPages(RemainingDocumenPagestIn remainingDocumenPagestIn)
        {
            List<int> pages = new List<int>();
            registerEventRepository.SaveRegisterEvent(remainingDocumenPagestIn.userId.Value, remainingDocumenPagestIn.key.Value, "Log - Start", "Repository.DocumentRepository.GetRemainingDocumentPages", "");

            using (var db = new DBContext())
            {
                pages.AddRange(db.SlicePages.Where(x => x.Active == true && x.DeletedDate == null && x.Slices.DocumentId == remainingDocumenPagestIn.documentId).Select(x => x.Page).ToList());
                pages.AddRange(db.DeletedPages.Where(x => x.Active == true && x.DeletedDate == null && x.DocumentId == remainingDocumenPagestIn.documentId).Select(x => x.Page).ToList());
            }

            registerEventRepository.SaveRegisterEvent(remainingDocumenPagestIn.userId.Value, remainingDocumenPagestIn.key.Value, "Log - End", "Repository.DocumentRepository.GetRemainingDocumentPages", "");
            return pages;
        }
    }
}

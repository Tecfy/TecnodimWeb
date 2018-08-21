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

        public DocumentOut GetDocumentById(DocumentIn documentIn)
        {
            DocumentOut documentOut = new DocumentOut();
            registerEventRepository.SaveRegisterEvent(documentIn.userId.Value, documentIn.key.Value, "Log - Start", "Repository.DocumentRepository.GetDocumentById", "");

            string externalId = string.Empty;

            using (var db = new DBContext())
            {
                externalId = db.Documents.Where(x => x.DocumentId == documentIn.documentId).FirstOrDefault().ExternalId;
            }

            documentOut = documentApi.GetDocument(externalId);

            registerEventRepository.SaveRegisterEvent(documentIn.userId.Value, documentIn.key.Value, "Log - End", "Repository.DocumentRepository.GetDocumentById", "");
            return documentOut;
        }

        public DocumentOut GetDocumentByHash(string hash)
        {
            DocumentOut documentOut = new DocumentOut();
            Guid guid = Guid.Parse(hash);
            string externalId = string.Empty;

            using (var db = new DBContext())
            {
                externalId = db.Documents.Where(x => x.Hash == guid).FirstOrDefault().ExternalId;
            }

            documentOut = documentApi.GetDocument(externalId);

            return documentOut;
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

﻿using ApiTecnodim;
using DataEF.DataAccess;
using Model.In;
using Model.Out;
using System.Collections.Generic;
using System.Linq;

namespace Repository
{
    public partial class DocumentRepository
    {
        RegisterEventRepository registerEventRepository = new RegisterEventRepository();
        DocumentApi documentApi = new DocumentApi();

        public DocumentsOut GetDocuments(DocumentsIn documentsIn)
        {
            DocumentsOut documentsOut = new DocumentsOut();
            registerEventRepository.SaveRegisterEvent(documentsIn.userId.Value, documentsIn.key.Value, "Log - Start", "Repository.DocumentRepository.GetDocuments", "");

            documentsOut = documentApi.GetDocuments(documentsIn);

            registerEventRepository.SaveRegisterEvent(documentsIn.userId.Value, documentsIn.key.Value, "Log - End", "Repository.DocumentRepository.GetDocuments", "");
            return documentsOut;
        }

        public List<int> GetRemainingDocumentPages(RemainingDocumenPagestIn remainingDocumenPagestIn)
        {
            List<int> pages = new List<int>();
            registerEventRepository.SaveRegisterEvent(remainingDocumenPagestIn.userId.Value, remainingDocumenPagestIn.key.Value, "Log - Start", "Repository.DocumentRepository.GetRemainingDocumentPages", "");

            using (var db = new DBContext())
            {
                pages.AddRange(db.ClippingPages.Where(x => x.Active == true && x.DeletedDate == null && x.Clippings.Documents.ExternalId == remainingDocumenPagestIn.externalId).Select(x => x.Page).ToList());
                pages.AddRange(db.DeletedPages.Where(x => x.Active == true && x.DeletedDate == null && x.Documents.ExternalId == remainingDocumenPagestIn.externalId).Select(x => x.Page).ToList());
            }

            registerEventRepository.SaveRegisterEvent(remainingDocumenPagestIn.userId.Value, remainingDocumenPagestIn.key.Value, "Log - End", "Repository.DocumentRepository.GetRemainingDocumentPages", "");
            return pages;
        }
    }
}

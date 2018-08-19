﻿using ApiTecnodim;
using DataEF.DataAccess;
using Model.In;
using Model.Out;
using System.Linq;

namespace Repository
{
    public partial class DocumentDetailRepository
    {
        RegisterEventRepository registerEventRepository = new RegisterEventRepository();
        DocumentRepository documentRepository = new DocumentRepository();
        DocumentDetailApi documentDetailApi = new DocumentDetailApi();

        public DocumentDetailOut GetDocumentDetail(DocumentDetailIn documentDetailIn)
        {
            DocumentDetailOut documentDetailOut = new DocumentDetailOut();
            registerEventRepository.SaveRegisterEvent(documentDetailIn.userId.Value, documentDetailIn.key.Value, "Log - Start", "Repository.DocumentDetailRepository.GetDocumentDetail", "");

            using (var db = new DBContext())
            {
                documentDetailIn.registration = db.Documents.Where(x => x.Active == true && x.DeletedDate == null && x.DocumentId == documentDetailIn.documentId).FirstOrDefault().Registration;
            }

            documentDetailOut = documentDetailApi.GetDocumentDetail(documentDetailIn);

            registerEventRepository.SaveRegisterEvent(documentDetailIn.userId.Value, documentDetailIn.key.Value, "Log - End", "Repository.DocumentDetailRepository.GetDocumentDetail", "");
            return documentDetailOut;
        }
    }
}

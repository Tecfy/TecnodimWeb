using ApiTecnodim;
using DataEF.DataAccess;
using Model.In;
using Model.Out;
using System;
using System.Linq;

namespace Repository
{
    public partial class DocumentDetailRepository
    {
        RegisterEventRepository registerEventRepository = new RegisterEventRepository();
        SliceRepository sliceRepository = new SliceRepository();
        DocumentDetailApi documentDetailApi = new DocumentDetailApi();
        DocumentDetailSERRepository documentDetailSERRepository = new DocumentDetailSERRepository();

        #region .: Api :.

        public DocumentDetailsByRegistrationOut GetDocumentDetailsByRegistration(DocumentDetailsByRegistrationIn documentsDetailIn)
        {
            DocumentDetailsByRegistrationOut documentDetailsByRegistrationOut = new DocumentDetailsByRegistrationOut();

            registerEventRepository.SaveRegisterEvent(documentsDetailIn.userId, documentsDetailIn.key, "Log - Start", "Repository.DocumentDetailRepository.GetDocumentsDetail", "");

            documentDetailsByRegistrationOut = documentDetailApi.GetDocumentDetailsByRegistration(documentsDetailIn.Registration, documentsDetailIn.Unity);

            registerEventRepository.SaveRegisterEvent(documentsDetailIn.userId, documentsDetailIn.key, "Log - End", "Repository.DocumentDetailRepository.GetDocumentsDetail", "");

            return documentDetailsByRegistrationOut;
        }

        public DocumentDetailOut GetDocumentDetail(DocumentDetailIn documentDetailIn)
        {
            DocumentDetailOut documentDetailOut = new DocumentDetailOut();
            registerEventRepository.SaveRegisterEvent(documentDetailIn.userId, documentDetailIn.key, "Log - Start", "Repository.DocumentDetailRepository.GetDocumentDetail", "");

            string registration = string.Empty;

            using (var db = new DBContext())
            {
                Documents document = db.Documents.Where(x => x.DocumentId == documentDetailIn.documentId).FirstOrDefault();

                if (document == null)
                {
                    throw new System.Exception(i18n.Resource.RegisterNotFound);
                }

                registration = document.Registration;
            }

            documentDetailOut = documentDetailApi.GetDocumentDetail(registration);

            SlicesOut slicesOut = new SlicesOut();

            slicesOut = sliceRepository.GetSlices(new SlicesIn { documentId = documentDetailIn.documentId, userId = documentDetailIn.userId, key = documentDetailIn.key, classificated = true });

            documentDetailOut.result.Classificated = slicesOut.result.Count;

            slicesOut = sliceRepository.GetSlices(new SlicesIn { documentId = documentDetailIn.documentId, userId = documentDetailIn.userId, key = documentDetailIn.key, classificated = false });

            documentDetailOut.result.NotClassificated = slicesOut.result.Count;

            registerEventRepository.SaveRegisterEvent(documentDetailIn.userId, documentDetailIn.key, "Log - End", "Repository.DocumentDetailRepository.GetDocumentDetail", "");
            return documentDetailOut;
        }

        public DocumentDetailsOut GetDocumentDetails(DocumentDetailsIn documentDetailsIn)
        {
            DocumentDetailsOut documentDetailsOut = new DocumentDetailsOut();
            registerEventRepository.SaveRegisterEvent(documentDetailsIn.userId, documentDetailsIn.key, "Log - Start", "Repository.DocumentDetailRepository.GetDocumentDetails", "");

            documentDetailSERRepository.GetDocumentDetailSER();

            registerEventRepository.SaveRegisterEvent(documentDetailsIn.userId, documentDetailsIn.key, "Log - End", "Repository.DocumentDetailRepository.GetDocumentDetails", "");
            return documentDetailsOut;
        }

        #endregion
    }
}

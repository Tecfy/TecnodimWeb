using DataEF.DataAccess;
using Helper.Enum;
using Model.In;
using Model.Out;
using Model.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using WebSupergoo.ABCpdf11;

namespace Repository
{
    public partial class PDFRepository
    {
        RegisterEventRepository registerEventRepository = new RegisterEventRepository();
        DocumentRepository documentRepository = new DocumentRepository();

        #region .: Methods :.

        public PDFsOut GetPDFs(DocumentIn documentIn)
        {
            PDFsOut pdfOut = new PDFsOut();
            registerEventRepository.SaveRegisterEvent(documentIn.id, documentIn.key, "Log - Start", "Repository.PDFRepository.GetPDFs", "");

            ECMDocumentOut documentOut = documentRepository.GetECMDocumentById(documentIn);

            RemainingDocumenPagestIn remainingDocumenPagestIn = new RemainingDocumenPagestIn() { documentId = documentIn.documentId, id = documentIn.id, key = documentIn.key };

            List<int> pages = new List<int>();
            pages = documentRepository.GetRemainingDocumentPages(remainingDocumenPagestIn);

            Doc theDoc = new Doc();
            theDoc.Read(Convert.FromBase64String(documentOut.result.archive));

            for (int i = 1; i <= theDoc.PageCount; i++)
            {
                if (!pages.Contains(i))
                {
                    pdfOut.result.Add(new PDFsVM()
                    {
                        page = i,
                        image = string.Format("/Images/GetImage/{0}/{1}", documentOut.result.hash, i),
                        thumb = string.Format("/Images/GetImage/{0}/{1}/true", documentOut.result.hash, i)
                    });
                }
            }

            theDoc.Clear();

            using (var db = new DBContext())
            {
                if (pdfOut.result == null || pdfOut.result.Count <= 0)
                {
                    Documents document = db.Documents.Where(x => x.DocumentId == documentIn.documentId).FirstOrDefault();

                    if (document.DocumentStatusId == (int)EDocumentStatus.PartiallySlice)
                    {
                        documentRepository.PostDocumentUpdateSatus(new DocumentUpdateIn { id = documentIn.id, key = documentIn.key, documentId = documentIn.documentId, documentStatusId = (int)EDocumentStatus.Slice });
                    }
                }
            }

            registerEventRepository.SaveRegisterEvent(documentIn.id, documentIn.key, "Log - End", "Repository.PDFRepository.GetPDFs", "");
            return pdfOut;
        }

        #endregion
    }
}

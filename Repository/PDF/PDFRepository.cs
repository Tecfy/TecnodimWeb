using ApiTecnodim;
using DataEF.DataAccess;
using Model.In;
using Model.Out;
using Model.VM;
using System;
using System.Linq;
using WebSupergoo.ABCpdf11;

namespace Repository.RegisterEvent
{
    public class PDFRepository
    {
        RegisterEventRepository registerEventRepository = new RegisterEventRepository();
        DocumentApi documentApi = new DocumentApi();

        #region .: Methods :.

        public PDFsOut GetPDFs(DocumentIn documentIn)
        {
            PDFsOut pdfOut = new PDFsOut();
            registerEventRepository.SaveRegisterEvent(documentIn.userId, documentIn.key, "Log - Start", "Repository.RegisterEvent.PDFRepository.GetPDFs", "");

            DocumentOut documentOut = documentApi.Get(documentIn);

            Doc theDoc = new Doc();
            theDoc.Read(Convert.FromBase64String(documentOut.result.archive));

            for (int i = 1; i <= theDoc.PageCount; i++)
            {
                pdfOut.result.Add(new PDFsVM() { pageId = i, image = "/Images?documentId=" + documentIn.documentId + "&pageId=" + i, thumb = "/Images?documentId=" + documentIn.documentId + "&pageId=" + i + "&thumb=true" });
            }

            theDoc.Clear();

            registerEventRepository.SaveRegisterEvent(documentIn.userId, documentIn.key, "Log - End", "Repository.RegisterEvent.PDFRepository.GetPDFs", "");
            return pdfOut;
        }

        public void Rotate(PDFIn pdfIn)
        {
            Doc theDoc = new Doc();
            //theDoc.Read(documentOut.result.archive);

            int theCount = theDoc.PageCount;
            string thePages = String.Join(",", pdfIn.pages.Select(x => x.pageId).ToList());
            theDoc.RemapPages(thePages);

            for (int p = 1; p <= theDoc.PageCount; p++)
            {
                theDoc.PageNumber = p;

                if (pdfIn.pages.Any(x => x.index == p && x.rotation != null && x.rotation > 0))
                    if (pdfIn.pages.Where(x => x.index == p).FirstOrDefault().rotation % 90 == 0)
                        theDoc.SetInfo(theDoc.Page, "/Rotate", pdfIn.pages.Where(x => x.index == p).FirstOrDefault().rotation.ToString());
            }

            theDoc.Save(@"C:\\Temp\\Tecnodim\\RemapPages.pdf");

            theDoc.Clear();
        }

        #endregion
    }
}

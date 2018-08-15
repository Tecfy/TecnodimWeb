﻿using ApiTecnodim;
using Model.In;
using Model.Out;
using Model.VM;
using System;
using System.Collections.Generic;
using System.Web.Configuration;
using WebSupergoo.ABCpdf11;

namespace Repository
{
    public partial class PDFRepository
    {
        RegisterEventRepository registerEventRepository = new RegisterEventRepository();
        DocumentRepository documentRepository = new DocumentRepository();
        DocumentApi documentApi = new DocumentApi();

        #region .: Methods :.

        public PDFsOut GetPDFs(DocumentIn documentIn)
        {
            PDFsOut pdfOut = new PDFsOut();
            registerEventRepository.SaveRegisterEvent(documentIn.userId.Value, documentIn.key.Value, "Log - Start", "Repository.PDFRepository.GetPDFs", "");

            DocumentOut documentOut = documentApi.GetDocument(documentIn);

            RemainingDocumenPagestIn remainingDocumenPagestIn = new RemainingDocumenPagestIn() { externalId = documentIn.externalId, userId = documentIn.userId, key = documentIn.key };

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
                        image = string.Format("/Images?externalId={0}&page={1}", documentIn.externalId, i),
                        thumb = string.Format("/Images?externalId={0}&page={1}&thumb=true", documentIn.externalId, i)
                    });
                }
            }

            theDoc.Clear();

            registerEventRepository.SaveRegisterEvent(documentIn.userId.Value, documentIn.key.Value, "Log - End", "Repository.PDFRepository.GetPDFs", "");
            return pdfOut;
        }

        public void Rotate(PDFIn pdfIn)
        {
            //TODO Ajustar o Método de Rotação
            /*
            Doc theDoc = new Doc();
            //theDoc.Read(documentOut.result.archive);

            int theCount = theDoc.PageCount;
            string thePages = String.Join(",", pdfIn.pages.Select(x => x.page).ToList());
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
            */
        }

        #endregion
    }
}

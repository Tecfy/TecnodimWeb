using System;
using System.Linq;
using System.Web.Http;
using Tecnodim.Models.In;
using Tecnodim.Models.Out;
using Tecnodim.Models.VM;
using WebSupergoo.ABCpdf11;

namespace Tecnodim.Controllers
{
    [RoutePrefix("api/PDFs")]
    public class PDFsController : ApiController
    {
        [Authorize]
        [HttpGet]
        [Route("")]
        public PDFsOut Get(int documentId)
        {
            PDFsOut pdfOut = new PDFsOut();

            try
            {
                byte[] arquivo = System.IO.File.ReadAllBytes(@"D:\\Rudolf\\Tecfy\\Tecnodim\\VICTOR - CONTRATOS.pdf");

                Doc theDoc = new Doc();
                theDoc.Read(arquivo);

                for (int i = 1; i <= theDoc.PageCount; i++)
                {
                    pdfOut.Result.Add(new PDFsVM() { PageId = i, Image = "/Images?documentId=" + documentId + "&pageId=" + i, Thumb = "/Images?documentId=" + documentId + "&pageId=" + i + "&thumb=true" });
                }

                theDoc.Clear();

                return pdfOut;
            }
            catch (Exception)
            {
                pdfOut.Result = null;
                pdfOut.SuccessMessage = null;
                pdfOut.Messages.Add(i18n.Resource.UnknownError);

                return pdfOut;
            }
        }

        [Authorize]
        [HttpPost]
        [Route("")]
        public PDFOut Post(PDFIn pdf)
        {
            PDFOut pdfOut = new PDFOut();

            try
            {
                byte[] arquivo = System.IO.File.ReadAllBytes(@"D:\\Rudolf\\Tecfy\\Tecnodim\\VICTOR - CONTRATOS.pdf");

                Doc theDoc = new Doc();
                theDoc.Read(arquivo);

                int theCount = theDoc.PageCount;
                string thePages = String.Join(",", pdf.Pages.Select(x => x.PageId).ToList());
                theDoc.RemapPages(thePages);

                for (int p = 1; p <= theDoc.PageCount; p++)
                {
                    theDoc.PageNumber = p;

                    if (pdf.Pages.Any(x => x.PageId == p && x.Rotation != null && x.Rotation > 0))
                        if (pdf.Pages.Where(x => x.PageId == p).FirstOrDefault().Rotation % 90 == 0)
                            theDoc.SetInfo(theDoc.Page, "/Rotate", pdf.Pages.Where(x => x.PageId == p).FirstOrDefault().Rotation.ToString());
                }

                theDoc.Save(@"C:\\Temp\\Tecnodim\\RemapPages.pdf");

                theDoc.Clear();

                return pdfOut;
            }
            catch (Exception)
            {
                pdfOut.SuccessMessage = null;
                pdfOut.Messages.Add(i18n.Resource.UnknownError);

                return pdfOut;
            }
        }
    }
}

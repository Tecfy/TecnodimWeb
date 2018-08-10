using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebSupergoo.ABCpdf11;
using WebSupergoo.ABCpdf11.Objects;

namespace Tecnodim.Controllers
{
    [RoutePrefix("api/Images")]
    public class ImagesController : ApiController
    {
        [Authorize]
        [Route("")]
        public HttpResponseMessage Get(int documentId, int pageId, bool thumb = false)
        {
            try
            {
                byte[] arquivo = System.IO.File.ReadAllBytes(@"D:\\Rudolf\\Tecfy\\Tecnodim\\VICTOR - CONTRATOS.pdf");

                Doc theDoc = new Doc();
                theDoc.Read(arquivo);

                Doc singlePagePdf = new Doc();
                singlePagePdf.Rect.String = singlePagePdf.MediaBox.String = theDoc.MediaBox.String;
                singlePagePdf.AddPage();
                singlePagePdf.AddImageDoc(theDoc, pageId, null);
                singlePagePdf.FrameRect();

                if (thumb)
                {
                    singlePagePdf.Rendering.DotsPerInch = 20;
                    Page[] pages = singlePagePdf.ObjectSoup.Catalog.Pages.GetPageArrayAll();
                    foreach (Page page in pages)
                    {
                        singlePagePdf.Page = page.ID;
                        using (XImage xi = XImage.FromData(singlePagePdf.Rendering.GetData(".jpg"), null))
                            page.Thumbnail = PixMap.FromXImage(singlePagePdf.ObjectSoup, xi);
                    }

                    byte[] theData = singlePagePdf.Rendering.GetData(".jpg");

                    MemoryStream ms = new MemoryStream(theData);
                    HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                    response.Content = new StreamContent(ms);
                    response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpg");

                    return response;
                }
                else
                {
                    byte[] theData = singlePagePdf.Rendering.GetData(".jpg");

                    MemoryStream ms = new MemoryStream(theData);
                    HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                    response.Content = new StreamContent(ms);
                    response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpg");

                    return response;
                }
            }
            catch (Exception)
            {
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                return response;
            }
        }
    }
}

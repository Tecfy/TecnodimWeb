using Model.In;
using Model.Out;
using Repository;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Site.Api.Controllers
{
    [RoutePrefix("Api/ScanningImages")]
    public class ScanningImagesController : ApiController
    {
        private ScanningImageRepository scanningImageRepository = new ScanningImageRepository();

        [HttpGet]
        public HttpResponseMessage GetImageScanning(string hash, int page, bool thumb = false)
        {
            ImageOut imageOut = new ImageOut();
            Guid Key = Guid.NewGuid();

            try
            {
                imageOut = scanningImageRepository.GetImageScanning(new ImageIn() { hash = hash, page = page, thumb = thumb });

                MemoryStream ms = new MemoryStream(imageOut.result.image);
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK) { Content = new StreamContent(ms) };
                response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpg");

                return response;
            }
            catch
            {
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.NotFound);
                return response;
            }
        }
    }
}

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
    [RoutePrefix("Api/Images")]
    public class ImagesController : ApiController
    {
        RegisterEventRepository registerEventRepository = new RegisterEventRepository();
        ImageRepository imageRepository = new ImageRepository();

        [HttpGet]
        public HttpResponseMessage GetImage(string hash, int page, bool thumb = false)
        {
            ImageOut imageOut = new ImageOut();
            Guid Key = Guid.NewGuid();

            try
            {
                imageOut = imageRepository.GetImage(new ImageIn() { hash = hash, page = page, thumb = thumb });

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

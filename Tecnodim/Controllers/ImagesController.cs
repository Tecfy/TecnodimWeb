using Microsoft.AspNet.Identity;
using Model.In;
using Model.Out;
using Repository;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Tecnodim.Controllers
{
    [RoutePrefix("api/images")]
    public class ImagesController : ApiController
    {
        RegisterEventRepository registerEventRepository = new RegisterEventRepository();
        ImageRepository imageRepository = new ImageRepository();

        [Authorize, HttpGet, Route("")]
        public HttpResponseMessage Get(int externalId, int page, bool thumb = false)
        {
            ImageOut imageOut = new ImageOut();
            Guid Key = Guid.NewGuid();

            try
            {
                imageOut = imageRepository.GetImage(new ImageIn() { externalId = externalId, userId = new Guid(User.Identity.GetUserId()), key = Key, page = page, thumb = thumb });

                MemoryStream ms = new MemoryStream(imageOut.result.image);
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StreamContent(ms);
                response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpg");

                return response;
            }
            catch (Exception ex)
            {
                registerEventRepository.SaveRegisterEvent(new Guid(User.Identity.GetUserId()), Key, "Erro", "Tecnodim.Controllers.ImagesController.Get", ex.Message);

                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.NotFound);
                return response;
            }
        }
    }
}

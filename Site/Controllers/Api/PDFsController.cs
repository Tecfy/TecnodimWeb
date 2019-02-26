using Microsoft.AspNet.Identity;
using Model.In;
using Model.Out;
using Repository;
using System;
using System.Web.Http;

namespace Site.Api.Controllers
{
    [RoutePrefix("Api/PDFs")]
    public class PDFsController : ApiController
    {
        private RegisterEventRepository registerEventRepository = new RegisterEventRepository();
        private PDFRepository pdfRepository = new PDFRepository();
        private DeletedPageRepository deletedPageRepository = new DeletedPageRepository();

        [Authorize(Roles = "Usuário"), HttpGet]
        public PDFsOut GetPDFs(int id)
        {
            PDFsOut pdfOut = new PDFsOut();
            string Key = Guid.NewGuid().ToString();

            try
            {
                DocumentIn documentIn = new DocumentIn() { documentId = id, id = User.Identity.GetUserId(), key = Key };

                pdfOut = pdfRepository.GetPDFs(documentIn);
            }
            catch (Exception ex)
            {
                registerEventRepository.SaveRegisterEvent(User.Identity.GetUserId(), Key, "Erro", "Tecnodim.Controllers.PDFsController.Get", ex.Message);

                pdfOut.result = null;
                pdfOut.successMessage = null;
                pdfOut.messages.Add(ex.Message);
            }

            return pdfOut;
        }
    }
}

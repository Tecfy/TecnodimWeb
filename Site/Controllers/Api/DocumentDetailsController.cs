using Microsoft.AspNet.Identity;
using Model.In;
using Model.Out;
using Repository;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ModelBinding;

namespace Site.Api.Controllers
{
    [RoutePrefix("Api/DocumentDetails")]
    public class DocumentDetailsController : ApiController
    {
        private RegisterEventRepository registerEventRepository = new RegisterEventRepository();
        private DocumentDetailRepository documentDetailRepository = new DocumentDetailRepository();

        [Authorize(Roles = "Usuário"), HttpGet]
        public DocumentDetailsByRegistrationOut GetDocumentDetailsByRegistration(string registration, string unity)
        {
            DocumentDetailsByRegistrationOut documentDetailsByRegistrationOut = new DocumentDetailsByRegistrationOut();
            string Key = Guid.NewGuid().ToString();

            try
            {
                DocumentDetailsByRegistrationIn documentDetailsByRegistrationIn = new DocumentDetailsByRegistrationIn() { Registration = registration, Unity = unity, id = User.Identity.GetUserId(), key = Key };

                documentDetailsByRegistrationOut = documentDetailRepository.GetDocumentDetailsByRegistration(documentDetailsByRegistrationIn);
            }
            catch (Exception ex)
            {
                registerEventRepository.SaveRegisterEvent(User.Identity.GetUserId(), Key, "Erro", "Tecnodim.Controllers.DocumentDetailsController.GetDocumentDetailsByRegistration", ex.Message);

                documentDetailsByRegistrationOut.result = null;
                documentDetailsByRegistrationOut.successMessage = null;
                documentDetailsByRegistrationOut.messages.Add(ex.Message);
            }

            return documentDetailsByRegistrationOut;
        }

        [Authorize(Roles = "Usuário"), HttpGet]
        public DocumentDetailOut GetDocumentDetailByDocumentId(int id)
        {
            DocumentDetailOut documentDetailOut = new DocumentDetailOut();
            string Key = Guid.NewGuid().ToString();

            try
            {
                DocumentDetailIn documentDetailIn = new DocumentDetailIn() { documentId = id, id = User.Identity.GetUserId(), key = Key };

                documentDetailOut = documentDetailRepository.GetDocumentDetail(documentDetailIn);
            }
            catch (Exception ex)
            {
                registerEventRepository.SaveRegisterEvent(User.Identity.GetUserId(), Key, "Erro", "Tecnodim.Controllers.DocumentDetailsController.GetDocumentDetail", ex.Message);

                documentDetailOut.result = null;
                documentDetailOut.successMessage = null;
                documentDetailOut.messages.Add(ex.Message);
            }

            return documentDetailOut;
        }

        [AllowAnonymous, HttpGet]
        public HttpResponseMessage GetDocumentDetails()
        {
            System.Threading.Tasks.Task objTask = System.Threading.Tasks.Task.Factory.StartNew(() =>
            {
                string Key = Guid.NewGuid().ToString();

                try
                {
                    DocumentDetailsIn documentDetailsIn = new DocumentDetailsIn() { key = Key };

                    documentDetailRepository.GetDocumentDetails(documentDetailsIn);
                }
                catch (Exception ex)
                {
                    registerEventRepository.SaveRegisterEvent("", Key, "Erro", "Tecnodim.Controllers.DocumentDetailsController.GetDocumentDetails", ex.Message);
                }
            });

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}

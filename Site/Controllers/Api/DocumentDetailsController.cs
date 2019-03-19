using Microsoft.AspNet.Identity;
using Model.In;
using Model.Out;
using Repository;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Site.Api.Controllers
{
    [RoutePrefix("Api/DocumentDetails")]
    public class DocumentDetailsController : ApiController
    {
        private RegisterEventRepository registerEventRepository = new RegisterEventRepository();
        private DocumentDetailRepository documentDetailRepository = new DocumentDetailRepository();

        [Authorize(Roles = "Usuário"), HttpGet]
        public DocumentDetailsByRegistrationOut GetDocumentDetailsByRegistration(string registration, int unityId)
        {
            DocumentDetailsByRegistrationOut documentDetailsByRegistrationOut = new DocumentDetailsByRegistrationOut();
            string Key = Guid.NewGuid().ToString();

            try
            {
                DocumentDetailsByRegistrationIn documentDetailsByRegistrationIn = new DocumentDetailsByRegistrationIn() { Registration = registration, UnityId = unityId, id = User.Identity.GetUserId(), key = Key };

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
        public DocumentDetailJobIdOut GetDocumentDetailByJobId(int id)
        {
            DocumentDetailJobIdOut documentDetailOut = new DocumentDetailJobIdOut();
            string Key = Guid.NewGuid().ToString();

            try
            {
                DocumentDetailByJobIdIn documentDetailIn = new DocumentDetailByJobIdIn() { jobId = id, id = User.Identity.GetUserId(), key = Key };

                documentDetailOut = documentDetailRepository.GetDocumentDetailByJobId(documentDetailIn);
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

        [Authorize(Roles = "Usuário"), HttpGet]
        public DocumentDetailDocumentIdOut GetDocumentDetailByDocumentId(int id)
        {
            DocumentDetailDocumentIdOut documentDetailOut = new DocumentDetailDocumentIdOut();
            string Key = Guid.NewGuid().ToString();

            try
            {
                DocumentDetailByDocumentIdIn documentDetailIn = new DocumentDetailByDocumentIdIn() { documentId = id, id = User.Identity.GetUserId(), key = Key };

                documentDetailOut = documentDetailRepository.GetDocumentDetailByDocumentId(documentDetailIn);
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

using Microsoft.AspNet.Identity;
using Model.In;
using Model.Out;
using Repository;
using System;
using System.Web.Http;

namespace Site.Api.Controllers
{
    [RoutePrefix("Api/ResendDocuments")]
    public class ResendDocumentsController : ApiController
    {
        private RegisterEventRepository registerEventRepository = new RegisterEventRepository();
        private ResendDocumentRepository resendDocumentRepository = new ResendDocumentRepository();

        [Authorize(Roles = "Usuário"), HttpGet]
        public ResendDocumentsOut GetResendDocuments(int unityId, string registration)
        {
            ResendDocumentsOut resendDocumentsOut = new ResendDocumentsOut();
            string Key = Guid.NewGuid().ToString();

            try
            {
                ResendDocumentsIn resendDocumentsIn = new ResendDocumentsIn() { unityId = unityId, registration = registration, id = User.Identity.GetUserId(), key = Key };

                resendDocumentsOut = resendDocumentRepository.GetResendDocuments(resendDocumentsIn);
            }
            catch (Exception ex)
            {
                registerEventRepository.SaveRegisterEvent(User.Identity.GetUserId(), Key, "Erro", "Tecnodim.Controllers.ResendDocumentsController.GetResendDocuments", ex.Message);

                resendDocumentsOut.successMessage = null;
                resendDocumentsOut.messages.Add(ex.Message);
            }

            return resendDocumentsOut;
        }
    }
}

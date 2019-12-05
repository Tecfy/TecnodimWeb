using Microsoft.AspNet.Identity;
using Model.In;
using Model.Out;
using Repository;
using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.ModelBinding;

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

        [Authorize(Roles = "Usuário"), HttpPost, Route("")]
        public ResendDocumentOut Post(ResendDocumentIn resendDocumentIn)
        {
            ResendDocumentOut resendDocumentOut = new ResendDocumentOut();
            string Key = Guid.NewGuid().ToString();

            try
            {
                if (ModelState.IsValid)
                {
                    if (resendDocumentIn.itens == null || resendDocumentIn.itens.Count() <= 0)
                    {
                        throw new Exception(i18n.Resource.DocumentsNotFound);
                    }
                    else
                    {
                        resendDocumentIn.id = User.Identity.GetUserId();
                        resendDocumentIn.key = Key;

                        resendDocumentOut = resendDocumentRepository.SaveResendDocument(resendDocumentIn);
                    }
                }
                else
                {
                    foreach (ModelState modelState in ModelState.Values)
                    {
                        var errors = modelState.Errors;
                        if (errors.Any())
                        {
                            foreach (ModelError error in errors)
                            {
                                throw new Exception(error.ErrorMessage);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                registerEventRepository.SaveRegisterEvent(User.Identity.GetUserId(), Key, "Erro", "Tecnodim.Controllers.ResendDocumentsController.Post", ex.Message);

                resendDocumentOut.successMessage = null;
                resendDocumentOut.messages.Add(ex.Message);
            }

            return resendDocumentOut;
        }
    }
}

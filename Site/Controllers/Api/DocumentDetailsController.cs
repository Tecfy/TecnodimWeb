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
    [RoutePrefix("Api/DocumentDetails")]
    public class DocumentDetailsController : ApiController
    {
        RegisterEventRepository registerEventRepository = new RegisterEventRepository();
        DocumentDetailRepository documentDetailRepository = new DocumentDetailRepository();

        [Authorize(Roles = "Usuário"), HttpGet]
        public DocumentDetailOut GetDocumentDetailByDocumentId(int id)
        {
            DocumentDetailOut documentDetailOut = new DocumentDetailOut();
            Guid Key = Guid.NewGuid();

            try
            {
                if (ModelState.IsValid)
                {
                    DocumentDetailIn documentDetailIn = new DocumentDetailIn() { documentId = id, userId = new Guid(User.Identity.GetUserId()), key = Key };

                    documentDetailOut = documentDetailRepository.GetDocumentDetail(documentDetailIn);
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
                registerEventRepository.SaveRegisterEvent(new Guid(User.Identity.GetUserId()), Key, "Erro", "Tecnodim.Controllers.DocumentDetailsController.GetDocumentDetail", ex.Message);

                documentDetailOut.result = null;
                documentDetailOut.successMessage = null;
                documentDetailOut.messages.Add(ex.Message);
            }

            return documentDetailOut;
        }
    }
}

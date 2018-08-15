using Microsoft.AspNet.Identity;
using Model.In;
using Model.Out;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ModelBinding;

namespace Tecnodim.Controllers
{
    [RoutePrefix("api/documents")]
    public class DocumentsController : ApiController
    {
        RegisterEventRepository registerEventRepository = new RegisterEventRepository();
        DocumentRepository documentRepository = new DocumentRepository();

        [Authorize, HttpGet]
        public DocumentsOut GetDocuments()
        {
            DocumentsOut documentsOut = new DocumentsOut();
            Guid Key = Guid.NewGuid();

            try
            {
                if (ModelState.IsValid)
                {
                    DocumentsIn documentsIn = new DocumentsIn() { userId = new Guid(User.Identity.GetUserId()), key = Key };

                    documentsOut = documentRepository.GetDocuments(documentsIn);
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
                registerEventRepository.SaveRegisterEvent(new Guid(User.Identity.GetUserId()), Key, "Erro", "Tecnodim.Controllers.DocumentsController.Get", ex.Message);

                documentsOut.successMessage = null;
                documentsOut.messages.Add(ex.Message);
            }

            return documentsOut;
        }
    }
}

using Helper.Enum;
using Microsoft.AspNet.Identity;
using Model.In;
using Model.Out;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.ModelBinding;

namespace Site.Controllers
{
    [RoutePrefix("Api/Documents")]
    public class DocumentsController : ApiController
    {
        RegisterEventRepository registerEventRepository = new RegisterEventRepository();
        DocumentRepository documentRepository = new DocumentRepository();

        [Authorize(Roles = "Usuário"), HttpGet]
        public ECMDocumentsOut GetECMDocuments()
        {
            ECMDocumentsOut ecmDocumentsOut = new ECMDocumentsOut();
            Guid Key = Guid.NewGuid();

            try
            {
                if (ModelState.IsValid)
                {
                    ECMDocumentsIn ecmDocumentsIn = new ECMDocumentsIn() { userId = new Guid(User.Identity.GetUserId()), key = Key };

                    ecmDocumentsOut = documentRepository.GetECMDocuments(ecmDocumentsIn);
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
                registerEventRepository.SaveRegisterEvent(new Guid(User.Identity.GetUserId()), Key, "Erro", "Tecnodim.Controllers.DocumentsController.GetECMDocuments", ex.Message);

                ecmDocumentsOut.successMessage = null;
                ecmDocumentsOut.messages.Add(ex.Message);
            }

            return ecmDocumentsOut;
        }

        [Authorize(Roles = "Usuário"), HttpGet]
        public ECMDocumentsSendOut GetECMSendDocuments()
        {
            ECMDocumentsSendOut ecmDocumentsSendOut = new ECMDocumentsSendOut();
            Guid Key = Guid.NewGuid();

            try
            {
                if (ModelState.IsValid)
                {
                    ECMDocumentsSendIn ecmDocumentsSendIn = new ECMDocumentsSendIn() { userId = new Guid(User.Identity.GetUserId()), key = Key };

                    ecmDocumentsSendOut = documentRepository.GetECMSendDocuments(ecmDocumentsSendIn);
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
                registerEventRepository.SaveRegisterEvent(new Guid(User.Identity.GetUserId()), Key, "Erro", "Tecnodim.Controllers.DocumentsController.GetECMSendDocuments", ex.Message);

                ecmDocumentsSendOut.successMessage = null;
                ecmDocumentsSendOut.messages.Add(ex.Message);
            }

            return ecmDocumentsSendOut;
        }

        [Authorize(Roles = "Usuário"), HttpGet]
        public DocumentsOut GetDocumentSlices(int id)
        {
            DocumentsOut documentsOut = new DocumentsOut();
            Guid Key = Guid.NewGuid();

            try
            {
                if (ModelState.IsValid)
                {
                    List<int> documentStatusIds = new List<int>();
                    documentStatusIds.Add((int)EDocumentStatus.New);
                    documentStatusIds.Add((int)EDocumentStatus.PartiallySlice);

                    DocumentsIn documentsIn = new DocumentsIn() { unityId = id, userId = new Guid(User.Identity.GetUserId()), key = Key, documentStatusIds = documentStatusIds };

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
                registerEventRepository.SaveRegisterEvent(new Guid(User.Identity.GetUserId()), Key, "Erro", "Tecnodim.Controllers.DocumentsController.GetDocumentSlices", ex.Message);

                documentsOut.successMessage = null;
                documentsOut.messages.Add(ex.Message);
            }

            return documentsOut;
        }

        [Authorize(Roles = "Usuário"), HttpGet]
        public DocumentsOut GetDocumentClassificateds(int id)
        {
            DocumentsOut documentsOut = new DocumentsOut();
            Guid Key = Guid.NewGuid();

            try
            {
                if (ModelState.IsValid)
                {
                    List<int> documentStatusIds = new List<int>();
                    documentStatusIds.Add((int)EDocumentStatus.Slice);
                    documentStatusIds.Add((int)EDocumentStatus.PartiallyClassificated);
                    documentStatusIds.Add((int)EDocumentStatus.Classificated);

                    DocumentsIn documentsIn = new DocumentsIn() { unityId = id, userId = new Guid(User.Identity.GetUserId()), key = Key, documentStatusIds = documentStatusIds };

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
                registerEventRepository.SaveRegisterEvent(new Guid(User.Identity.GetUserId()), Key, "Erro", "Tecnodim.Controllers.DocumentsController.GetDocumentClassificateds", ex.Message);

                documentsOut.successMessage = null;
                documentsOut.messages.Add(ex.Message);
            }

            return documentsOut;
        }

        [Authorize(Roles = "Usuário"), HttpPost]
        public DocumentUpdateOut PostDocumentUpdateSatus(DocumentUpdateIn documentUpdateIn)
        {
            DocumentUpdateOut documentUpdateOut = new DocumentUpdateOut();
            Guid Key = Guid.NewGuid();

            try
            {
                if (ModelState.IsValid)
                {
                    documentUpdateIn.userId = new Guid(User.Identity.GetUserId());
                    documentUpdateIn.key = Key;

                    documentUpdateOut = documentRepository.PostDocumentUpdateSatus(documentUpdateIn);
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
                registerEventRepository.SaveRegisterEvent(new Guid(User.Identity.GetUserId()), Key, "Erro", "Tecnodim.Controllers.DocumentsController.PostDocumentUpdateSatus", ex.Message);

                documentUpdateOut.successMessage = null;
                documentUpdateOut.messages.Add(ex.Message);
            }

            return documentUpdateOut;
        }
    }
}

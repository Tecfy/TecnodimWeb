using Helper.Enum;
using Microsoft.AspNet.Identity;
using Model.In;
using Model.Out;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.ModelBinding;

namespace Site.Api.Controllers
{
    [RoutePrefix("Api/Documents")]
    public class DocumentsController : ApiController
    {
        private RegisterEventRepository registerEventRepository = new RegisterEventRepository();
        private DocumentRepository documentRepository = new DocumentRepository();

        [AllowAnonymous, HttpGet]
        public HttpResponseMessage GetECMDocuments()
        {
            var currentContext = HttpContext.Current;

            System.Threading.Tasks.Task objTask = System.Threading.Tasks.Task.Factory.StartNew(() =>
            {
                HttpContext.Current = currentContext;
                string Key = Guid.NewGuid().ToString();

                try
                {
                    ECMDocumentsIn ecmDocumentsIn = new ECMDocumentsIn() { id = "", key = Key };

                    documentRepository.GetECMDocuments(ecmDocumentsIn);

                }
                catch (Exception ex)
                {
                    registerEventRepository.SaveRegisterEvent("", Key, "Erro", "Tecnodim.Controllers.DocumentsController.GetECMDocuments", ex.Message);
                }
            });

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [AllowAnonymous, HttpGet]
        public HttpResponseMessage GetECMSendDocuments()
        {
            var currentContext = HttpContext.Current;

            System.Threading.Tasks.Task objTask = System.Threading.Tasks.Task.Factory.StartNew(() =>
            {
                HttpContext.Current = currentContext;
                string Key = Guid.NewGuid().ToString();

                try
                {
                    ECMDocumentsSendIn ecmDocumentsSendIn = new ECMDocumentsSendIn() { id = "", key = Key };

                    documentRepository.GetECMSendDocuments(ecmDocumentsSendIn);
                }
                catch (Exception ex)
                {
                    registerEventRepository.SaveRegisterEvent("", Key, "Erro", "Tecnodim.Controllers.DocumentsController.GetECMSendDocuments", ex.Message);
                }
            });

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [AllowAnonymous, HttpGet]
        public HttpResponseMessage GetECMValidateDocuments()
        {
            var currentContext = HttpContext.Current;

            System.Threading.Tasks.Task objTask = System.Threading.Tasks.Task.Factory.StartNew(() =>
            {
                HttpContext.Current = currentContext;
                string Key = Guid.NewGuid().ToString();

                try
                {
                    ECMDocumentsValidateIn ecmDocumentsValidateIn = new ECMDocumentsValidateIn() { id = "", key = Key };

                    documentRepository.GetECMValidateDocuments(ecmDocumentsValidateIn);
                }
                catch (Exception ex)
                {
                    registerEventRepository.SaveRegisterEvent("", Key, "Erro", "Tecnodim.Controllers.DocumentsController.GetECMValidateDocuments", ex.Message);
                }
            });

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [AllowAnonymous, HttpGet]
        public HttpResponseMessage GetECMValidateAdInterfaceDocuments()
        {
            var currentContext = HttpContext.Current;

            System.Threading.Tasks.Task objTask = System.Threading.Tasks.Task.Factory.StartNew(() =>
            {
                HttpContext.Current = currentContext;
                string Key = Guid.NewGuid().ToString();

                try
                {
                    ECMDocumentsValidateAdInterfaceIn eCMDocumentsValidateAdInterfaceIn = new ECMDocumentsValidateAdInterfaceIn() { id = "", key = Key };

                    documentRepository.GetECMValidateAdInterfaceDocuments(eCMDocumentsValidateAdInterfaceIn);
                }
                catch (Exception ex)
                {
                    registerEventRepository.SaveRegisterEvent("", Key, "Erro", "Tecnodim.Controllers.DocumentsController.GetECMValidateAdInterfaceDocuments", ex.Message);
                }
            });

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [Authorize(Roles = "Usuário"), HttpGet]
        public DocumentsOut GetDocumentSlices(int unityId, string registration = null, string name = null, string externalId = null, int documentId = 0, int documentStatusId = 0, int currentPage = 1, int qtdEntries = 50)
        {
            DocumentsOut documentsOut = new DocumentsOut();
            string Key = Guid.NewGuid().ToString();

            try
            {
                List<int> documentStatusIds = new List<int>();
                documentStatusIds.Add((int)EDocumentStatus.New);
                documentStatusIds.Add((int)EDocumentStatus.PartiallySlice);

                DocumentsIn documentsIn = new DocumentsIn() { unityId = unityId, registration = registration, name = name, externalId = externalId, documentId = documentId, documentStatusId = documentStatusId, id = User.Identity.GetUserId(), key = Key, documentStatusIds = documentStatusIds, currentPage = currentPage, qtdEntries = qtdEntries };

                documentsOut = documentRepository.GetDocuments(documentsIn);
            }
            catch (Exception ex)
            {
                registerEventRepository.SaveRegisterEvent(User.Identity.GetUserId(), Key, "Erro", "Tecnodim.Controllers.DocumentsController.GetDocumentSlices", ex.Message);

                documentsOut.successMessage = null;
                documentsOut.messages.Add(ex.Message);
            }

            return documentsOut;
        }

        [Authorize(Roles = "Usuário"), HttpGet]
        public DocumentsOut GetDocumentClassificateds(int unityId, string registration = null, string name = null, string externalId = null, int documentId = 0, int documentStatusId = 0, int currentPage = 1, int qtdEntries = 50)
        {
            DocumentsOut documentsOut = new DocumentsOut();
            string Key = Guid.NewGuid().ToString();

            try
            {
                List<int> documentStatusIds = new List<int>();
                documentStatusIds.Add((int)EDocumentStatus.Slice);
                documentStatusIds.Add((int)EDocumentStatus.PartiallyClassificated);
                documentStatusIds.Add((int)EDocumentStatus.Classificated);

                DocumentsIn documentsIn = new DocumentsIn() { unityId = unityId, registration = registration, name = name, externalId = externalId, documentId = documentId, documentStatusId = documentStatusId, id = User.Identity.GetUserId(), key = Key, documentStatusIds = documentStatusIds, currentPage = currentPage, qtdEntries = qtdEntries };

                documentsOut = documentRepository.GetDocuments(documentsIn);
            }
            catch (Exception ex)
            {
                registerEventRepository.SaveRegisterEvent(User.Identity.GetUserId(), Key, "Erro", "Tecnodim.Controllers.DocumentsController.GetDocumentClassificateds", ex.Message);

                documentsOut.successMessage = null;
                documentsOut.messages.Add(ex.Message);
            }

            return documentsOut;
        }

        [Authorize(Roles = "Usuário"), HttpPost]
        public DocumentUpdateOut PostDocumentUpdateSatus(DocumentUpdateIn documentUpdateIn)
        {
            DocumentUpdateOut documentUpdateOut = new DocumentUpdateOut();
            string Key = Guid.NewGuid().ToString();

            try
            {
                if (ModelState.IsValid)
                {
                    documentUpdateIn.id = User.Identity.GetUserId();
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
                registerEventRepository.SaveRegisterEvent(User.Identity.GetUserId(), Key, "Erro", "Tecnodim.Controllers.DocumentsController.PostDocumentUpdateSatus", ex.Message);

                documentUpdateOut.successMessage = null;
                documentUpdateOut.messages.Add(ex.Message);
            }

            return documentUpdateOut;
        }

        [AllowAnonymous, HttpGet]
        public HttpResponseMessage ConvertDocumentPB(string file)
        {
            var currentContext = HttpContext.Current;

            System.Threading.Tasks.Task objTask = System.Threading.Tasks.Task.Factory.StartNew(() =>
            {
                HttpContext.Current = currentContext;
                documentRepository.ConvertDocumentPB(file);
            });

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [HttpGet]
        public DocumentValidateSliceOut GetDocumentValidateSlice(int id)
        {
            DocumentValidateSliceOut documentValidateSliceOut = new DocumentValidateSliceOut();
            string Key = Guid.NewGuid().ToString();

            try
            {
                DocumentValidateSliceIn documentValidateSliceIn = new DocumentValidateSliceIn() { documentId = id, id = User.Identity.GetUserId(), key = Key };

                documentValidateSliceOut = documentRepository.GetDocumentValidateSlice(documentValidateSliceIn);
            }
            catch (Exception ex)
            {
                registerEventRepository.SaveRegisterEvent(User.Identity.GetUserId(), Key, "Erro", "Tecnodim.Controllers.DocumentsController.GetDocumentValidateSlice", ex.Message);

                documentValidateSliceOut.successMessage = null;
                documentValidateSliceOut.messages.Add(ex.Message);
            }

            return documentValidateSliceOut;
        }

        [HttpGet]
        public DocumentValidateClassificationOut GetDocumentValidateClassification(int id)
        {
            DocumentValidateClassificationOut documentValidateClassificationOut = new DocumentValidateClassificationOut();
            string Key = Guid.NewGuid().ToString();

            try
            {
                DocumentValidateClassificationIn documentValidateClassificationIn = new DocumentValidateClassificationIn() { documentId = id, id = User.Identity.GetUserId(), key = Key };

                documentValidateClassificationOut = documentRepository.GetDocumentValidateClassification(documentValidateClassificationIn);
            }
            catch (Exception ex)
            {
                registerEventRepository.SaveRegisterEvent(User.Identity.GetUserId(), Key, "Erro", "Tecnodim.Controllers.DocumentsController.GetDocumentValidateClassification", ex.Message);

                documentValidateClassificationOut.successMessage = null;
                documentValidateClassificationOut.messages.Add(ex.Message);
            }

            return documentValidateClassificationOut;
        }

        [HttpGet]
        public DocumentsFinishedOut GetDocumentsFinished()
        {
            DocumentsFinishedOut documentsFinishedOut = new DocumentsFinishedOut();
            string Key = Guid.NewGuid().ToString();

            try
            {
                DocumentsFinishedIn documentsFinishedIn = new DocumentsFinishedIn() { id = User.Identity.GetUserId(), key = Key };

                documentsFinishedOut = documentRepository.GetDocumentsFinished(documentsFinishedIn);
            }
            catch (Exception ex)
            {
                registerEventRepository.SaveRegisterEvent(User.Identity.GetUserId(), Key, "Erro", "Tecnodim.Controllers.DocumentsController.GetDocumentsFinished", ex.Message);

                documentsFinishedOut.successMessage = null;
                documentsFinishedOut.messages.Add(ex.Message);
            }

            return documentsFinishedOut;
        }
    }
}

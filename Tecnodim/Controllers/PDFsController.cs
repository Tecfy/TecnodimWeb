﻿using Microsoft.AspNet.Identity;
using Model.In;
using Model.Out;
using Repository;
using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.ModelBinding;

namespace Tecnodim.Controllers
{
    [RoutePrefix("api/PDFs")]
    public class PDFsController : ApiController
    {
        RegisterEventRepository registerEventRepository = new RegisterEventRepository();
        PDFRepository pdfRepository = new PDFRepository();
        DeletedPageRepository deletedPageRepository = new DeletedPageRepository();

        [Authorize, HttpGet, Route("")]
        public PDFsOut Get(int documentId)
        {
            PDFsOut pdfOut = new PDFsOut();
            Guid Key = Guid.NewGuid();

            try
            {
                if (ModelState.IsValid)
                {
                    DocumentIn documentIn = new DocumentIn() { documentId = documentId, userId = new Guid(User.Identity.GetUserId()), key = Key };

                    pdfOut = pdfRepository.GetPDFs(documentIn);
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
                registerEventRepository.SaveRegisterEvent(new Guid(User.Identity.GetUserId()), Key, "Erro", "Tecnodim.Controllers.PDFsController.Get", ex.Message);

                pdfOut.result = null;
                pdfOut.successMessage = null;
                pdfOut.messages.Add(ex.Message);
            }

            return pdfOut;
        }
    }
}
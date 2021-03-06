﻿using Microsoft.AspNet.Identity;
using Model.In;
using Model.Out;
using Repository;
using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.ModelBinding;

namespace Site.Api.Controllers
{
    [RoutePrefix("Api/Classifications")]
    public class ClassificationsController : ApiController
    {
        private RegisterEventRepository registerEventRepository = new RegisterEventRepository();
        private ClassificationRepository classificationRepository = new ClassificationRepository();

        [Authorize(Roles = "Usuário"), HttpPost, Route("")]
        public ClassificationOut Post(ClassificationIn classificationIn)
        {
            ClassificationOut sliceOut = new ClassificationOut();
            string Key = Guid.NewGuid().ToString();

            try
            {
                if (ModelState.IsValid)
                {
                    classificationIn.id = User.Identity.GetUserId();
                    classificationIn.key = Key;

                    sliceOut = classificationRepository.SaveClassification(classificationIn);
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
                registerEventRepository.SaveRegisterEvent(User.Identity.GetUserId(), Key, "Erro", "Tecnodim.Controllers.ClassificationsController.Post", ex.Message);

                sliceOut.successMessage = null;
                sliceOut.messages.Add(ex.Message);
            }

            return sliceOut;
        }
    }
}

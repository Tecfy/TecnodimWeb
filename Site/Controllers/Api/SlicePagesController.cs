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
    [RoutePrefix("Api/SlicePages")]
    public class SlicePagesController : ApiController
    {
        private RegisterEventRepository registerEventRepository = new RegisterEventRepository();
        private SlicePageRepository slicePageRepository = new SlicePageRepository();

        [Authorize(Roles = "Usuário"), HttpPost, Route("")]
        public SlicePageDeleteOut Post(SlicePageDeleteIn slicePageDeleteIn)
        {
            SlicePageDeleteOut slicePageOut = new SlicePageDeleteOut();
            string Key = Guid.NewGuid().ToString();

            try
            {
                if (ModelState.IsValid)
                {
                    slicePageDeleteIn.id = User.Identity.GetUserId();
                    slicePageDeleteIn.key = Key;

                    slicePageRepository.DeleteSlicePage(slicePageDeleteIn);
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
                registerEventRepository.SaveRegisterEvent(User.Identity.GetUserId(), Key, "Erro", "Tecnodim.Controllers.SlicePagesController.Post", ex.Message);

                slicePageOut.successMessage = null;
                slicePageOut.messages.Add(ex.Message);
            }

            return slicePageOut;
        }
    }
}

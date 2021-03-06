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
    [RoutePrefix("Api/Slices")]
    public class SlicesController : ApiController
    {
        private RegisterEventRepository registerEventRepository = new RegisterEventRepository();
        private SliceRepository sliceRepository = new SliceRepository();

        #region .: API :.

        #region .: Get :.

        [Authorize(Roles = "Usuário"), HttpGet]
        public SliceOut GetSliceById(int id)
        {
            SliceOut sliceOut = new SliceOut();
            string Key = Guid.NewGuid().ToString();

            try
            {
                SliceIn sliceIn = new SliceIn() { sliceId = id, id = User.Identity.GetUserId(), key = Key };

                sliceOut = sliceRepository.GetSlice(sliceIn);
            }
            catch (Exception ex)
            {
                registerEventRepository.SaveRegisterEvent(User.Identity.GetUserId(), Key, "Erro", "Tecnodim.Controllers.SlicesController.GetSlice", ex.Message);

                sliceOut.successMessage = null;
                sliceOut.messages.Add(ex.Message);
            }

            return sliceOut;
        }

        [Authorize(Roles = "Usuário"), HttpGet]
        public SliceOut GetSlicePending(int id)
        {
            SliceOut sliceOut = new SliceOut();
            string Key = Guid.NewGuid().ToString();

            try
            {
                SlicePendingIn slicePendingIn = new SlicePendingIn() { documentId = id, id = User.Identity.GetUserId(), key = Key };

                sliceOut = sliceRepository.GetSlicePending(slicePendingIn);
            }
            catch (Exception ex)
            {
                registerEventRepository.SaveRegisterEvent(User.Identity.GetUserId(), Key, "Erro", "Tecnodim.Controllers.SlicesController.GetSlicePending", ex.Message);

                sliceOut.successMessage = null;
                sliceOut.messages.Add(ex.Message);
            }

            return sliceOut;
        }

        [Authorize(Roles = "Usuário"), HttpGet]
        public SlicesOut GetSlicesByDocumentId(int id)
        {
            SlicesOut slicesOut = new SlicesOut();
            string Key = Guid.NewGuid().ToString();

            try
            {
                SlicesIn slicesIn = new SlicesIn() { documentId = id, id = User.Identity.GetUserId(), key = Key, classificated = null };

                slicesOut = sliceRepository.GetSlices(slicesIn);
            }
            catch (Exception ex)
            {
                registerEventRepository.SaveRegisterEvent(User.Identity.GetUserId(), Key, "Erro", "Tecnodim.Controllers.SlicesController.GetSlices", ex.Message);

                slicesOut.successMessage = null;
                slicesOut.messages.Add(ex.Message);
            }

            return slicesOut;
        }

        [Authorize(Roles = "Usuário"), HttpGet]
        public SlicesOut GetSlicesNotClassificatedByDocumentId(int id)
        {
            SlicesOut slicesOut = new SlicesOut();
            string Key = Guid.NewGuid().ToString();

            try
            {
                SlicesIn slicesIn = new SlicesIn() { documentId = id, id = User.Identity.GetUserId(), key = Key, classificated = false };

                slicesOut = sliceRepository.GetSlices(slicesIn);
            }
            catch (Exception ex)
            {
                registerEventRepository.SaveRegisterEvent(User.Identity.GetUserId(), Key, "Erro", "Tecnodim.Controllers.SlicesController.GetSlices", ex.Message);

                slicesOut.successMessage = null;
                slicesOut.messages.Add(ex.Message);
            }

            return slicesOut;
        }

        [Authorize(Roles = "Usuário"), HttpGet]
        public SlicesOut GetSlicesClassificatedByDocumentId(int id)
        {
            SlicesOut slicesOut = new SlicesOut();
            string Key = Guid.NewGuid().ToString();

            try
            {
                SlicesIn slicesIn = new SlicesIn() { documentId = id, id = User.Identity.GetUserId(), key = Key, classificated = true };

                slicesOut = sliceRepository.GetSlices(slicesIn);
            }
            catch (Exception ex)
            {
                registerEventRepository.SaveRegisterEvent(User.Identity.GetUserId(), Key, "Erro", "Tecnodim.Controllers.SlicesController.GetSlices", ex.Message);

                slicesOut.successMessage = null;
                slicesOut.messages.Add(ex.Message);
            }

            return slicesOut;
        }

        #endregion

        #region .: Post :.

        [Authorize(Roles = "Usuário"), HttpPost]
        public SliceOut PostSliceMoveNew(SliceMoveNewIn sliceMoveNewIn)
        {
            SliceOut sliceOut = new SliceOut();
            string Key = Guid.NewGuid().ToString();

            try
            {
                if (ModelState.IsValid)
                {
                    sliceMoveNewIn.id = User.Identity.GetUserId();
                    sliceMoveNewIn.key = Key;
                    
                    sliceOut = sliceRepository.MoveSliceNew(sliceMoveNewIn);
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

                return sliceOut;
            }
            catch (Exception ex)
            {
                registerEventRepository.SaveRegisterEvent(User.Identity.GetUserId(), Key, "Erro", "Tecnodim.Controllers.SlicesController.PostSliceMoveNew", ex.Message);

                sliceOut.successMessage = null;
                sliceOut.messages.Add(ex.Message);

                return sliceOut;
            }
        }

        [Authorize(Roles = "Usuário"), HttpPost]
        public SliceOut PostSliceMoveExisting(SliceMoveExistingIn sliceMoveExistingIn)
        {
            SliceOut sliceOut = new SliceOut();
            string Key = Guid.NewGuid().ToString();

            try
            {
                if (ModelState.IsValid)
                {
                    sliceMoveExistingIn.id = User.Identity.GetUserId();
                    sliceMoveExistingIn.key = Key;

                    sliceOut = sliceRepository.MoveSliceExisting(sliceMoveExistingIn);
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

                return sliceOut;
            }
            catch (Exception ex)
            {
                registerEventRepository.SaveRegisterEvent(User.Identity.GetUserId(), Key, "Erro", "Tecnodim.Controllers.SlicesController.PostSliceMoveExisting", ex.Message);

                sliceOut.successMessage = null;
                sliceOut.messages.Add(ex.Message);

                return sliceOut;
            }
        }

        [Authorize(Roles = "Usuário"), HttpPost]
        public SliceOut PostSliceDelete(SliceDeleteIn sliceDeleteIn)
        {
            SliceOut sliceOut = new SliceOut();
            string Key = Guid.NewGuid().ToString();

            try
            {
                if (ModelState.IsValid)
                {
                    sliceDeleteIn.id = User.Identity.GetUserId();
                    sliceDeleteIn.key = Key;

                    sliceOut = sliceRepository.DeleteSlice(sliceDeleteIn);
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

                return sliceOut;
            }
            catch (Exception ex)
            {
                registerEventRepository.SaveRegisterEvent(User.Identity.GetUserId(), Key, "Erro", "Tecnodim.Controllers.SlicesController.SliceMoveDeleteIn", ex.Message);

                sliceOut.successMessage = null;
                sliceOut.messages.Add(ex.Message);

                return sliceOut;
            }
        }

        [Authorize(Roles = "Usuário"), HttpPost, Route("")]
        public SliceOut Post(SliceSaveIn sliceIn)
        {
            SliceOut sliceOut = new SliceOut();
            string Key = Guid.NewGuid().ToString();

            try
            {
                if (ModelState.IsValid)
                {
                    if (sliceIn.pages == null || sliceIn.pages.Count() <= 0)
                    {
                        throw new Exception(i18n.Resource.PagesNotFound);
                    }
                    else
                    {
                        sliceIn.id = User.Identity.GetUserId();
                        sliceIn.key = Key;

                        sliceOut = sliceRepository.SaveSlice(sliceIn);
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

                return sliceOut;
            }
            catch (Exception ex)
            {
                registerEventRepository.SaveRegisterEvent(User.Identity.GetUserId(), Key, "Erro", "Tecnodim.Controllers.SlicesController.Post", ex.Message);

                sliceOut.successMessage = null;
                sliceOut.messages.Add(ex.Message);

                return sliceOut;
            }
        }

        #endregion

        #endregion
    }
}

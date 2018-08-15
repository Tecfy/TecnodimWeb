using Microsoft.AspNet.Identity;
using Model.In;
using Model.Out;
using Repository;
using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.ModelBinding;

namespace Tecnodim.Controllers
{
    [RoutePrefix("api/slices")]
    public class SlicesController : ApiController
    {
        RegisterEventRepository registerEventRepository = new RegisterEventRepository();
        SliceRepository sliceRepository = new SliceRepository();

        [Authorize, HttpGet]
        public SliceOut GetSlice(int sliceId)
        {
            SliceOut sliceOut = new SliceOut();
            Guid Key = Guid.NewGuid();

            try
            {
                if (ModelState.IsValid)
                {
                    SliceIn sliceIn = new SliceIn() { sliceId = sliceId, userId = new Guid(User.Identity.GetUserId()), key = Key };

                    sliceOut = sliceRepository.GetSlice(sliceIn);
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
                registerEventRepository.SaveRegisterEvent(new Guid(User.Identity.GetUserId()), Key, "Erro", "Tecnodim.Controllers.SlicesController.GetSlice", ex.Message);

                sliceOut.successMessage = null;
                sliceOut.messages.Add(ex.Message);
            }

            return sliceOut;
        }

        [Authorize, HttpGet]
        public SlicesOut GetSlices(int documentId)
        {
            SlicesOut slicesOut = new SlicesOut();
            Guid Key = Guid.NewGuid();

            try
            {
                if (ModelState.IsValid)
                {
                    SlicesIn slicesIn = new SlicesIn() { documentId = documentId, userId = new Guid(User.Identity.GetUserId()), key = Key, classificated = null };

                    slicesOut = sliceRepository.GetSlices(slicesIn);
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
                registerEventRepository.SaveRegisterEvent(new Guid(User.Identity.GetUserId()), Key, "Erro", "Tecnodim.Controllers.SlicesController.GetSlices", ex.Message);

                slicesOut.successMessage = null;
                slicesOut.messages.Add(ex.Message);
            }

            return slicesOut;
        }

        [Authorize, HttpGet]
        public SlicesOut GetSlicesNotClassificated(int documentId)
        {
            SlicesOut slicesOut = new SlicesOut();
            Guid Key = Guid.NewGuid();

            try
            {
                if (ModelState.IsValid)
                {
                    SlicesIn slicesIn = new SlicesIn() { documentId = documentId, userId = new Guid(User.Identity.GetUserId()), key = Key, classificated = false };

                    slicesOut = sliceRepository.GetSlices(slicesIn);
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
                registerEventRepository.SaveRegisterEvent(new Guid(User.Identity.GetUserId()), Key, "Erro", "Tecnodim.Controllers.SlicesController.GetSlices", ex.Message);

                slicesOut.successMessage = null;
                slicesOut.messages.Add(ex.Message);
            }

            return slicesOut;
        }

        [Authorize, HttpGet]
        public SlicesOut GetSlicesClassificated(int documentId)
        {
            SlicesOut slicesOut = new SlicesOut();
            Guid Key = Guid.NewGuid();

            try
            {
                if (ModelState.IsValid)
                {
                    SlicesIn slicesIn = new SlicesIn() { documentId = documentId, userId = new Guid(User.Identity.GetUserId()), key = Key, classificated = true };

                    slicesOut = sliceRepository.GetSlices(slicesIn);
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
                registerEventRepository.SaveRegisterEvent(new Guid(User.Identity.GetUserId()), Key, "Erro", "Tecnodim.Controllers.SlicesController.GetSlices", ex.Message);

                slicesOut.successMessage = null;
                slicesOut.messages.Add(ex.Message);
            }

            return slicesOut;
        }

        [Authorize, HttpPost, Route("")]
        public SliceOut Post(SliceSaveIn sliceIn)
        {
            SliceOut sliceOut = new SliceOut();
            Guid Key = Guid.NewGuid();

            try
            {
                if (ModelState.IsValid)
                {
                    sliceIn.userId = new Guid(User.Identity.GetUserId());
                    sliceIn.key = Key;

                    sliceOut = sliceRepository.SaveSlice(sliceIn);
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
                registerEventRepository.SaveRegisterEvent(new Guid(User.Identity.GetUserId()), Key, "Erro", "Tecnodim.Controllers.SlicesController.Post", ex.Message);

                sliceOut.successMessage = null;
                sliceOut.messages.Add(ex.Message);

                return sliceOut;
            }
        }
    }
}

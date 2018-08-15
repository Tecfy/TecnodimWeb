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
    [RoutePrefix("api/students")]
    public class StudentsController : ApiController
    {
        RegisterEventRepository registerEventRepository = new RegisterEventRepository();
        StudentRepository studentRepository = new StudentRepository();

        [Authorize, HttpGet]
        public StudentOut GetStudent(int externalId)
        {
            StudentOut studentOut = new StudentOut();
            Guid Key = Guid.NewGuid();

            try
            {
                if (ModelState.IsValid)
                {
                    StudentIn studentIn = new StudentIn() { externalId = externalId, userId = new Guid(User.Identity.GetUserId()), key = Key };

                    studentOut = studentRepository.GetStudent(studentIn);
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
                registerEventRepository.SaveRegisterEvent(new Guid(User.Identity.GetUserId()), Key, "Erro", "Tecnodim.Controllers.StudentsController.GetStudent", ex.Message);

                studentOut.result = null;
                studentOut.successMessage = null;
                studentOut.messages.Add(ex.Message);
            }

            return studentOut;
        }
    }
}

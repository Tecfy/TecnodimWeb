using ApiTecnodim;
using Model.In;
using Model.Out;

namespace Repository
{
    public partial class StudentRepository
    {
        RegisterEventRepository registerEventRepository = new RegisterEventRepository();
        StudentApi studentApi = new StudentApi();

        public StudentOut GetStudent(StudentIn studentIn)
        {
            StudentOut studentOut = new StudentOut();
            registerEventRepository.SaveRegisterEvent(studentIn.userId.Value, studentIn.key.Value, "Log - Start", "Repository.StudentRepository.GetStudent", "");

            studentOut = studentApi.GetStudent(studentIn);

            registerEventRepository.SaveRegisterEvent(studentIn.userId.Value, studentIn.key.Value, "Log - End", "Repository.StudentRepository.GetStudent", "");
            return studentOut;
        }
    }
}

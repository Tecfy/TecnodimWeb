using Model.VM;

namespace Model.Out
{
    public class StudentOut : ResultServiceVM
    {
        public StudentOut()
        {
            this.result = new StudentVM();
        }

        public StudentVM result { get; set; }
    }
}

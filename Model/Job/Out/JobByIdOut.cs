using Model.VM;

namespace Model.Out
{
    public class JobByIdOut : ResultServiceVM
    {
        public JobByIdOut()
        {
            result = new JobByIdVM();
        }

        public JobByIdVM result { get; set; }
    }
}

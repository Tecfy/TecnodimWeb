using Model.VM;

namespace Model.Out
{
    public class JobByIdOut : ResultServiceVM
    {
        public JobByIdOut()
        {
            this.result = new JobByIdVM();
        }

        public JobByIdVM result { get; set; }
    }
}

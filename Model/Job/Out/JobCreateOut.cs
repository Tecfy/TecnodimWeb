using Model.VM;

namespace Model.Out
{
    public class JobCreateOut : ResultServiceVM
    {
        public JobCreateOut()
        {
            this.result = new JobCreateVM();
        }

        public JobCreateVM result { get; set; }
    }
}

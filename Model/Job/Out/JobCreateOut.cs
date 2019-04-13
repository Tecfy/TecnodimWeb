using Model.VM;

namespace Model.Out
{
    public class JobCreateOut : ResultServiceVM
    {
        public JobCreateOut()
        {
            result = new JobCreateVM();
        }

        public JobCreateVM result { get; set; }
    }
}

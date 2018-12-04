using Model.VM;

namespace Model.Out
{
    public class JobCategoryCreateOut : ResultServiceVM
    {
        public JobCategoryCreateOut()
        {
            this.result = new JobCategoryCreateVM();
        }

        public JobCategoryCreateVM result { get; set; }
    }
}

using Model.VM;

namespace Model.Out
{
    public class JobCategoryCreateOut : ResultServiceVM
    {
        public JobCategoryCreateOut()
        {
            result = new JobCategoryCreateVM();
        }

        public JobCategoryCreateVM result { get; set; }
    }
}

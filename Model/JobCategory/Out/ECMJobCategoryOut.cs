using Model.VM;

namespace Model.Out
{
    public class ECMJobCategoryOut : ResultServiceVM
    {
        public ECMJobCategoryOut()
        {
            this.result = new ECMJobCategoryVM();
        }

        public ECMJobCategoryVM result { get; set; }
    }
}

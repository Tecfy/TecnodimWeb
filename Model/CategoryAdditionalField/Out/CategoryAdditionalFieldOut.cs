using Model.VM;

namespace Model.Out
{
    public class CategoryAdditionalFieldOut : ResultServiceVM
    {
        public CategoryAdditionalFieldOut()
        {
            this.result = new CategoryAdditionalFieldVM();
        }

        public CategoryAdditionalFieldVM result { get; set; }
    }
}

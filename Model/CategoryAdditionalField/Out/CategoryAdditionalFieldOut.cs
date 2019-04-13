using Model.VM;

namespace Model.Out
{
    public class CategoryAdditionalFieldOut : ResultServiceVM
    {
        public CategoryAdditionalFieldOut()
        {
            result = new CategoryAdditionalFieldVM();
        }

        public CategoryAdditionalFieldVM result { get; set; }
    }
}

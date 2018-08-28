using Model.VM;

namespace Model.Out
{
    public class CategoryAdditionalFieldEditOut : ResultServiceVM
    {
        public CategoryAdditionalFieldEditOut()
        {
            this.result = new CategoryAdditionalFieldEditVM();
        }

        public CategoryAdditionalFieldEditVM result { get; set; }
    }
}

using Model.VM;

namespace Model.Out
{
    public class CategoryEditOut : ResultServiceVM
    {
        public CategoryEditOut()
        {
            this.result = new CategoryEditVM();
        }

        public CategoryEditVM result { get; set; }
    }
}

using Model.VM;

namespace Model.Out
{
    public class CategoryEditOut : ResultServiceVM
    {
        public CategoryEditOut()
        {
            result = new CategoryEditVM();
        }

        public CategoryEditVM result { get; set; }
    }
}

using Model.VM;

namespace Model.Out
{
    public class CategoryOut : ResultServiceVM
    {
        public CategoryOut()
        {
            result = new CategoryVM();
        }

        public CategoryVM result { get; set; }
    }
}

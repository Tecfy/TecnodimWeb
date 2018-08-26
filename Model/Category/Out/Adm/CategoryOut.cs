using Model.VM;

namespace Model.Out
{
    public class CategoryOut : ResultServiceVM
    {
        public CategoryOut()
        {
            this.result = new CategoryVM();
        }

        public CategoryVM result { get; set; }
    }
}

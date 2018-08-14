using Model.VM;
using System.Collections.Generic;

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

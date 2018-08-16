using Model.VM;
using System.Collections.Generic;

namespace Model.Out
{
    public class CategorySearchOut : ResultServiceVM
    {
        public CategorySearchOut()
        {
            this.result = new CategorySearchVM();
        }

        public CategorySearchVM result { get; set; }
    }
}

using Model.VM;
using System.Collections.Generic;

namespace Model.Out
{
    public class ApiCategoryOut : ResultServiceVM
    {
        public ApiCategoryOut()
        {
            this.result = new ApiCategoryVM();
        }

        public ApiCategoryVM result { get; set; }
    }
}

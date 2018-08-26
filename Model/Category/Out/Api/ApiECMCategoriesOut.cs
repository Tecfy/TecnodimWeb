using Model.VM;
using System.Collections.Generic;

namespace Model.Out
{
    public class ApiECMCategoriesOut : ResultServiceVM
    {
        public ApiECMCategoriesOut()
        {
            this.result = new List<ApiECMCategoriesVM>();
        }

        public List<ApiECMCategoriesVM> result { get; set; }
    }
}

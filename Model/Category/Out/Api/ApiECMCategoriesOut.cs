using Model.VM;
using System.Collections.Generic;

namespace Model.Out
{
    public class ApiECMCategoriesOut : ResultServiceVM
    {
        public ApiECMCategoriesOut()
        {
            result = new List<ApiECMCategoriesVM>();
        }

        public List<ApiECMCategoriesVM> result { get; set; }
    }
}

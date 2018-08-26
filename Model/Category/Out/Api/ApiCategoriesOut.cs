using Model.VM;
using System.Collections.Generic;

namespace Model.Out
{
    public class ApiCategoriesOut : ResultServiceVM
    {
        public ApiCategoriesOut()
        {
            this.result = new List<ApiCategoriesVM>();
        }

        public List<ApiCategoriesVM> result { get; set; }
    }
}

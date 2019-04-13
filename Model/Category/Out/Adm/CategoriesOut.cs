using Model.VM;
using System.Collections.Generic;

namespace Model.Out
{
    public class CategoriesOut : ResultServiceVM
    {
        public CategoriesOut()
        {
            result = new List<CategoriesVM>();
        }

        public List<CategoriesVM> result { get; set; }

        public int? totalCount { get; set; }
    }
}

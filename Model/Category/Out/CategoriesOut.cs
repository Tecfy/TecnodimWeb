using Model.VM;
using System.Collections.Generic;

namespace Model.Out
{
    public class CategoriesOut : ResultServiceVM
    {
        public CategoriesOut()
        {
            this.result = new List<CategoriesVM>();
        }

        public List<CategoriesVM> result { get; set; }
    }
}

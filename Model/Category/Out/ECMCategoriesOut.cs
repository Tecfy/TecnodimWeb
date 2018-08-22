using Model.VM;
using System.Collections.Generic;

namespace Model.Out
{
    public class ECMCategoriesOut : ResultServiceVM
    {
        public ECMCategoriesOut()
        {
            this.result = new List<ECMCategoriesVM>();
        }

        public List<ECMCategoriesVM> result { get; set; }
    }
}

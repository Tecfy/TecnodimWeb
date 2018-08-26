using Model.VM;
using System.Collections.Generic;

namespace Model.Out
{
    public class CategoriesDDLOut : ResultServiceVM
    {
        public CategoriesDDLOut()
        {
            this.result = new List<CategoriesDDLVM>();
        }

        public List<CategoriesDDLVM> result { get; set; }
    }
}

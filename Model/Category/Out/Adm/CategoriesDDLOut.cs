using Model.VM;
using System.Collections.Generic;

namespace Model.Out
{
    public class CategoriesDDLOut : ResultServiceVM
    {
        public CategoriesDDLOut()
        {
            result = new List<CategoriesDDLVM>();
        }

        public List<CategoriesDDLVM> result { get; set; }
    }
}

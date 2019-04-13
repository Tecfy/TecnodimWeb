using Model.VM;
using System.Collections.Generic;

namespace Model.Out
{
    public class UnitsOut : ResultServiceVM
    {
        public UnitsOut()
        {
            result = new List<UnitsVM>();
        }

        public List<UnitsVM> result { get; set; }

        public int? totalCount { get; set; }
    }
}

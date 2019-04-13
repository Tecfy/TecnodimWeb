using Model.VM;
using System.Collections.Generic;

namespace Model.Out
{
    public class UnitsDDLOut : ResultServiceVM
    {
        public UnitsDDLOut()
        {
            result = new List<UnitsDDLVM>();
        }

        public List<UnitsDDLVM> result { get; set; }
    }
}

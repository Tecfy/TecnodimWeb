using Model.VM;
using System.Collections.Generic;

namespace Model.Out
{
    public class SlicePagesOut : ResultServiceVM
    {
        public SlicePagesOut()
        {
            result = new List<SlicePagesVM>();
        }

        public List<SlicePagesVM> result { get; set; }
    }
}

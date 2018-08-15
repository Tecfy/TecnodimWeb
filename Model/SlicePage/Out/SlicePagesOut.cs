using Model.VM;
using System.Collections.Generic;

namespace Model.Out
{
    public class SlicePagesOut : ResultServiceVM
    {
        public SlicePagesOut()
        {
            this.result = new List<SlicePagesVM>();
        }

        public List<SlicePagesVM> result { get; set; }
    }
}

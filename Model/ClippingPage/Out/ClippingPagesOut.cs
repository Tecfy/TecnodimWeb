using Model.VM;
using System.Collections.Generic;

namespace Model.Out
{
    public class ClippingPagesOut : ResultServiceVM
    {
        public ClippingPagesOut()
        {
            this.result = new List<ClippingPagesVM>();
        }

        public List<ClippingPagesVM> result { get; set; }
    }
}

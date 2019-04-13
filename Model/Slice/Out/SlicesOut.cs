using Model.VM;
using System.Collections.Generic;

namespace Model.Out
{
    public class SlicesOut : ResultServiceVM
    {
        public SlicesOut()
        {
            result = new List<SlicesVM>();
        }

        public List<SlicesVM> result { get; set; }
    }
}

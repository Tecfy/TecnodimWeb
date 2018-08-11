using Model.VM;
using System.Collections.Generic;

namespace Model.Out
{
    public class ClippingsOut : resultServiceVM
    {
        public ClippingsOut()
        {
            this.result = new List<ClippingsVM>();
        }

        public List<ClippingsVM> result { get; set; }
    }
}

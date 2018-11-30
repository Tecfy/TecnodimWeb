using Model.VM;
using System.Collections.Generic;

namespace Model.Out
{
    public class SyncRuntimesOut : ResultServiceVM
    {
        public SyncRuntimesOut()
        {
            this.result = new List<SyncRuntimesVM>();
        }

        public List<SyncRuntimesVM> result { get; set; }
    }
}

using Model.VM;
using System.Collections.Generic;

namespace Model.Out
{
    public class WorkOut : ResultServiceVM
    {
        public WorkOut()
        {
            this.result = new WorkVM();
        }

        public WorkVM result { get; set; }
    }
}

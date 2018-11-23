using Model.VM;
using System.Collections.Generic;

namespace Model.Out
{
    public class JobOut : ResultServiceVM
    {
        public JobOut()
        {
            this.result = new JobVM();
        }

        public JobVM result { get; set; }
    }
}

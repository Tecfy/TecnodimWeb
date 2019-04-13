using Model.VM;
using System.Collections.Generic;

namespace Model.Out
{
    public class JobsSentOut : ResultServiceVM
    {
        public JobsSentOut()
        {
            result = new List<JobsSentVM>();
        }

        public List<JobsSentVM> result { get; set; }
    }
}

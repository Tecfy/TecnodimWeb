using Model.VM;
using System.Collections.Generic;

namespace Model.Out
{
    public class JobsByUserOut : ResultServiceVM
    {
        public JobsByUserOut()
        {
            this.result = new List<JobsByUserVM>();
        }

        public List<JobsByUserVM> result { get; set; }
    }
}

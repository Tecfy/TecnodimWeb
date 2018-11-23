using Model.VM;
using System.Collections.Generic;

namespace Model.Out
{
    public class JobsOut : ResultServiceVM
    {
        public JobsOut()
        {
            this.result = new List<JobsVM>();
        }

        public List<JobsVM> result { get; set; }

        public int? totalCount { get; set; }
    }
}

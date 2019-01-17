using Model.VM;
using System.Collections.Generic;

namespace Model.Out
{
    public class JobsFinishedOut : ResultServiceVM
    {
        public JobsFinishedOut()
        {
            this.result = new List<JobsFinishedVM>();
        }

        public List<JobsFinishedVM> result { get; set; }
    }
}

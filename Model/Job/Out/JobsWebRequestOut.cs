using Model.VM;
using System.Collections.Generic;

namespace Model.Out
{
    public class JobsWebRequestOut : ResultServiceVM
    {
        public JobsWebRequestOut()
        {
            this.result = new List<JobsWebRequestVM>();
        }

        public List<JobsWebRequestVM> result { get; set; }
    }
}

using Model.VM;
using System.Collections.Generic;

namespace Model.Out
{
    public class JobsRegistrationOut : ResultServiceVM
    {
        public JobsRegistrationOut()
        {
            this.result = new List<JobsRegistrationVM>();
        }

        public List<JobsRegistrationVM> result { get; set; }
    }
}

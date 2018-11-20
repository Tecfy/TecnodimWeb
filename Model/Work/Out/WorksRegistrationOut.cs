using Model.VM;
using System.Collections.Generic;

namespace Model.Out
{
    public class WorksRegistrationOut : ResultServiceVM
    {
        public WorksRegistrationOut()
        {
            this.result = new List<WorksRegistrationVM>();
        }

        public List<WorksRegistrationVM> result { get; set; }
    }
}

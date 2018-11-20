using Model.VM;
using System.Collections.Generic;

namespace Model.Out
{
    public class WorksOut : ResultServiceVM
    {
        public WorksOut()
        {
            this.result = new List<WorksVM>();
        }

        public List<WorksVM> result { get; set; }

        public int? totalCount { get; set; }
    }
}

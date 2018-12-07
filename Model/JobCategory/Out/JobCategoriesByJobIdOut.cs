using Model.VM;
using System.Collections.Generic;

namespace Model.Out
{
    public class JobCategoriesByJobIdOut : ResultServiceVM
    {
        public JobCategoriesByJobIdOut()
        {
            this.result = new List<JobCategoriesByJobIdVM>();
        }

        public List<JobCategoriesByJobIdVM> result { get; set; }
    }
}

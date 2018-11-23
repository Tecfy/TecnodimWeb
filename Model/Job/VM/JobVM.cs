using System;
using System.Collections.Generic;

namespace Model.VM
{
    public class JobVM
    {
        public int JobId { get; set; }

        public string Code { get; set; }

        public string Registration { get; set; }

        public DateTime CreatedDate { get; set; }


        public List<JobCategoryVM> JobCategories { get; set; }
    }
}

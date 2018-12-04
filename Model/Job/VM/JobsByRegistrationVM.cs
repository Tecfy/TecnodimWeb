using System;
using System.Collections.Generic;

namespace Model.VM
{
    public class JobsByRegistrationVM
    {
        public int JobId { get; set; }

        public string Registration { get; set; }

        public string Name { get; set; }


        public List<JobCategoriesByRegistrationVM> JobCategories { get; set; }
    }
}

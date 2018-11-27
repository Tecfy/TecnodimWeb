using System;
using System.Collections.Generic;

namespace Model.VM
{
    public class JobsRegistrationVM
    {
        public int JobId { get; set; }

        public string Registration { get; set; }

        public string Name { get; set; }


        public List<JobCategoriesRegistrationVM> JobCategories { get; set; }
    }
}

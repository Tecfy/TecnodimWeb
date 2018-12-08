using System.Collections.Generic;

namespace Model.VM
{
    public class JobCategoriesByJobIdVM
    {
        public int JobCategoryId { get; set; }

        public string category { get; set; }

        public bool received { get; set; }

        public bool send { get; set; }

        public List<JobCategoryPagesVM> jobCategoryPages { get; set; }

        public List<JobCategoryAdditionalFieldVM> additionalFields { get; set; }
    }
}

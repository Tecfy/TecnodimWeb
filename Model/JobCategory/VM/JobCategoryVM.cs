using System;

namespace Model.VM
{
    public class JobCategoryVM
    {
        public int JobCategoryId { get; set; }
    
        public string Category { get; set; }

        public string Code { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}

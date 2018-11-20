using System;
using System.Collections.Generic;

namespace Model.VM
{
    public class WorksVM
    {
        public int WorkId { get; set; }

        public string Code { get; set; }

        public string Registration { get; set; }
        
        public DateTime CreatedDate { get; set; }


        public List<WorkCategoryVM> WorkCategories { get; set; }
    }
}

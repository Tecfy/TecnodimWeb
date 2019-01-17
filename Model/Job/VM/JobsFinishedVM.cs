using System.Collections.Generic;

namespace Model.VM
{
    public class JobsFinishedVM
    {
        public int jobId { get; set; }

        public int jobCategoryId { get; set; }

        public string externalId { get; set; }

        public string registration { get; set; }

        public string categoryId { get; set; }

        public string category { get; set; }

        public bool pb { get; set; }
        
        public string title { get; set; }
        
        public List<AdditionalFieldSaveVM> additionalFields { get; set; }
    }
}

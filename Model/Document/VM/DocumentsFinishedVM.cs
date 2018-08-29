using System.Collections.Generic;

namespace Model.VM
{
    public class DocumentsFinishedVM
    {
        public int documentId { get; set; }

        public int sliceId { get; set; }

        public string externalId { get; set; }

        public string registration { get; set; }

        public string categoryId { get; set; }

        public string category { get; set; }
        
        public string title { get; set; }

        public List<SlicePagesFinishedVM> pages { get; set; }
    }
}

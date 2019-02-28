using System;
using System.Collections.Generic;

namespace Model.VM
{
    public class DocumentsFinishedVM
    {
        public string registration { get; set; }

        public string categoryId { get; set; }

        public int documentId { get; set; }

        public int sliceId { get; set; }

        public string externalId { get; set; }

        public string title { get; set; }

        public bool pb { get; set; }

        public string user { get; set; }

        public string sliceUser { get; set; }

        public string sliceUserRegistration { get; set; }

        public string classificationUser { get; set; }

        public string classificationUserRegistration { get; set; }

        public string extension { get; set; }

        public DateTime classificationDate { get; set; }

        public DateTime sliceDate { get; set; }

        public List<SlicePagesFinishedVM> pages { get; set; }

        public List<AdditionalFieldSaveVM> additionalFields { get; set; }
    }
}

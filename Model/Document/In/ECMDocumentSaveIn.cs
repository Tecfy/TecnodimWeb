using System;
using System.Collections.Generic;

namespace Model.In
{
    public class ECMDocumentSaveIn : BaseIn
    {
        public string registration { get; set; }

        public string categoryId { get; set; }

        public string archive { get; set; }

        public string title { get; set; }

        public string user { get; set; }

        public string sliceUser { get; set; }

        public string sliceUserRegistration { get; set; }

        public string classificationUser { get; set; }

        public string classificationUserRegistration { get; set; }

        public string extension { get; set; }

        public DateTime classificationDate { get; set; }

        public DateTime sliceDate { get; set; }

        public List<AdditionalFieldSaveIn> additionalFields { get; set; }
    }
}

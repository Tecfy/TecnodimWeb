using System.Collections.Generic;

namespace Model.In
{
    public class ECMDocumentSaveIn : BaseIn
    {
        public string registration { get; set; }

        public string categoryId { get; set; }

        public string category { get; set; }

        public string archive { get; set; }

        public string title { get; set; }

        public List<AdditionalFieldSaveIn> additionalFields { get; set; }
    }
}

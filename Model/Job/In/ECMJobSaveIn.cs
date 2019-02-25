using System.Collections.Generic;

namespace Model.In
{
    public class ECMJobSaveIn : BaseIn
    {
        public string registration { get; set; }

        public string categoryId { get; set; }

        public string archive { get; set; }

        public string title { get; set; }

        public string user { get; set; }

        public string extension { get; set; }

        public List<AdditionalFieldSaveIn> additionalFields { get; set; }
    }
}

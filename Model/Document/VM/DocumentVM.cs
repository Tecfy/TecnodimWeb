using System;

namespace Model.VM
{
    public class DocumentVM
    {
        public int DocumentId { get; set; }

        public string ExternalId { get; set; }

        public Guid Hash { get; set; }

        public int? Pages { get; set; }

        public bool Download { get; set; }
    }
}

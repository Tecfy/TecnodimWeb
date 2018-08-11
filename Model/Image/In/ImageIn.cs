using System;
using System.ComponentModel.DataAnnotations;

namespace Model.In
{
    public class ImageIn
    {
        public int documentId { get; set; }

        public Guid userId { get; set; }

        public Guid key { get; set; }

        public int pageId { get; set; }

        public bool thumb { get; set; }
    }
}

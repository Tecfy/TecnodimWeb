using System;
using System.ComponentModel.DataAnnotations;

namespace Model.In
{
    public class DocumentIn
    {
        public int documentId { get; set; }

        public Guid userId { get; set; }

        public Guid key { get; set; }
    }
}

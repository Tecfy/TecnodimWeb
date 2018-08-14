using System;
using System.ComponentModel.DataAnnotations;

namespace Model.In
{
    public class ImageIn : BaseIn
    {
        public int documentId { get; set; }

        public int page { get; set; }

        public bool thumb { get; set; }
    }
}

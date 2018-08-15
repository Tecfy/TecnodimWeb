using System;
using System.ComponentModel.DataAnnotations;

namespace Model.In
{
    public class ImageIn : BaseIn
    {
        public int externalId { get; set; }

        public int page { get; set; }

        public bool thumb { get; set; }
    }
}

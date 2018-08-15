using System.Collections.Generic;

namespace Model.VM
{
    public class ClippingVM
    {
        public int clippingId { get; set; }

        public int? categoryId { get; set; }

        public string name { get; set; }

        public bool classification { get; set; }

        public List<ClippingPageVM> clippingPages { get; set; }
    }
}

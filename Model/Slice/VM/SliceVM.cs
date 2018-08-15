using System.Collections.Generic;

namespace Model.VM
{
    public class SliceVM
    {
        public int sliceId { get; set; }

        public int? categoryId { get; set; }

        public string name { get; set; }
        
        public List<SlicePageVM> slicePages { get; set; }
        
        public List<AdditionalFieldVM> additionalFields { get; set; }
    }
}

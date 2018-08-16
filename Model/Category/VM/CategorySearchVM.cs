using Newtonsoft.Json;
using System.Collections.Generic;

namespace Model.VM
{
    public class CategorySearchVM
    {
        public int categoryId { get; set; }

        [JsonIgnore]
        public int? parentId { get; set; }

        public string name { get; set; }

        public List<string> parents { get; set; }


        public List<AdditionalFieldVM> additionalFields { get; set; }
    }
}

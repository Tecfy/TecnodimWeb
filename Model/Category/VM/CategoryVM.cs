using Newtonsoft.Json;
using System.Collections.Generic;

namespace Model.VM
{
    public class CategoryVM
    {
        public int categoryId { get; set; }

        [JsonIgnore]
        public int? parentId { get; set; }

        public string code { get; set; }

        public string name { get; set; }

        public List<string> parents { get; set; }


        public List<AdditionalFieldVM> additionalFields { get; set; }
    }
}

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Model.In
{
    public class DocumentsIn : BaseIn
    {
        public int unityId { get; set; }

        public string registration { get; set; }

        public string name { get; set; }

        public List<int> documentStatusIds { get; set; }
    }
}

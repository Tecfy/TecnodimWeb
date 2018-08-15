using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Model.In
{
    public class DocumentsIn : BaseIn
    {
        public List<int> documentStatusIds { get; set; }
    }
}

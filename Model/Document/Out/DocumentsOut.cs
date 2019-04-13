using Model.VM;
using System.Collections.Generic;

namespace Model.Out
{
    public class DocumentsOut : ResultServiceVM
    {
        public DocumentsOut()
        {
            result = new List<DocumentsVM>();
        }

        public List<DocumentsVM> result { get; set; }

        public int? totalCount { get; set; }
    }
}

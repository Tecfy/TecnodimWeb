using Model.VM;
using System.Collections.Generic;

namespace Model.Out
{
    public class DocumentsDetailOut : ResultServiceVM
    {
        public DocumentsDetailOut()
        {
            this.result = new List<DocumentsDetailVM>();
        }

        public List<DocumentsDetailVM> result { get; set; }
    }
}

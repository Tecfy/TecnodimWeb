using Model.VM;
using System.Collections.Generic;

namespace Model.Out
{
    public class DocumentsOut : ResultServiceVM
    {
        public DocumentsOut()
        {
            this.result = new List<DocumentsVM>();
        }

        public List<DocumentsVM> result { get; set; }
    }
}

using Model.VM;
using System.Collections.Generic;

namespace Model.Out
{
    public class DocumentsOut : resultServiceVM
    {
        public DocumentsOut()
        {
            this.result = new List<DocumentsVM>();
        }

        public List<DocumentsVM> result { get; set; }
    }
}

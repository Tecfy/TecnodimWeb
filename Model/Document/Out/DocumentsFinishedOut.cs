using Model.VM;
using System.Collections.Generic;

namespace Model.Out
{
    public class DocumentsFinishedOut : ResultServiceVM
    {
        public DocumentsFinishedOut()
        {
            result = new List<DocumentsFinishedVM>();
        }

        public List<DocumentsFinishedVM> result { get; set; }
    }
}

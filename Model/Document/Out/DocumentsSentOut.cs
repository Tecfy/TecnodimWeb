using Model.VM;
using System.Collections.Generic;

namespace Model.Out
{
    public class DocumentsSentOut : ResultServiceVM
    {
        public DocumentsSentOut()
        {
            result = new List<DocumentsSentVM>();
        }

        public List<DocumentsSentVM> result { get; set; }
    }
}

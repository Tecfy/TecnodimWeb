using Model.VM;
using System.Collections.Generic;

namespace Model.Out
{
    public class ResendDocumentsOut : ResultServiceVM
    {
        public ResendDocumentsOut()
        {
            result = new List<ResendDocumentsVM>();
        }

        public List<ResendDocumentsVM> result { get; set; }

        public int? totalCount { get; set; }
    }
}

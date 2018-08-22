using Model.VM;
using System.Collections.Generic;

namespace Model.Out
{
    public class ECMDocumentsOut : ResultServiceVM
    {
        public ECMDocumentsOut()
        {
            this.result = new List<ECMDocumentsVM>();
        }

        public List<ECMDocumentsVM> result { get; set; }
    }
}

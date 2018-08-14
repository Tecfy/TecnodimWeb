using Model.VM;
using System.Collections.Generic;

namespace Model.Out
{
    public class PDFsOut : ResultServiceVM
    {
        public PDFsOut()
        {
            this.result = new List<PDFsVM>();
        }

        public List<PDFsVM> result { get; set; }
    }
}

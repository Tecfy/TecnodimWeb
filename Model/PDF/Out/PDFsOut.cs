using Model.VM;
using System.Collections.Generic;

namespace Model.Out
{
    public class PDFsOut : ResultServiceVM
    {
        public PDFsOut()
        {
            result = new List<PDFsVM>();
        }

        public List<PDFsVM> result { get; set; }
    }
}

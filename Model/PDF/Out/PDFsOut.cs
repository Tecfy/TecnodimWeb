using Model.VM;
using System.Collections.Generic;

namespace Model.Out
{
    public class PDFsOut : resultServiceVM
    {
        public PDFsOut()
        {
            this.result = new List<PDFsVM>();
        }

        public List<PDFsVM> result { get; set; }
    }
}

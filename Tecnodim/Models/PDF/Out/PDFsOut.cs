using System.Collections.Generic;
using Tecnodim.Models.Common;
using Tecnodim.Models.VM;

namespace Tecnodim.Models.Out
{
    public class PDFsOut : ResultServiceVM
    {
        public PDFsOut()
        {
            this.Result = new List<PDFsVM>();
        }

        public List<PDFsVM> Result { get; set; }
    }
}
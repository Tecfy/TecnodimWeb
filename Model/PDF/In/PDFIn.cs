using Model.VM;
using System.Collections.Generic;

namespace Model.In
{
    public class PDFIn
    {
        public string archive { get; set; }

        public List<SlicePagesFinishedVM> pages { get; set; }
    }
}

using Model.VM;
using System.Collections.Generic;

namespace Model.In
{
    public class PDFIn
    {
        public string archive { get; set; }

        public bool pb { get; set; }

        public List<SlicePagesFinishedVM> pages { get; set; }
    }
}

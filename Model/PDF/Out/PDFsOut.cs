using Model.VM;

namespace Model.Out
{
    public class PDFsOut : ResultServiceVM
    {
        public PDFsOut()
        {
            result = new PDFsVM();
        }

        public PDFsVM result { get; set; }
    }
}

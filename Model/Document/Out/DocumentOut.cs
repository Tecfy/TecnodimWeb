using Model.Common;
using Model.VM;

namespace Model.Out
{
    public class DocumentOut : resultServiceVM
    {
        public DocumentOut()
        {
            this.result = new DocumentVM();
        }

        public DocumentVM result { get; set; }
    }
}

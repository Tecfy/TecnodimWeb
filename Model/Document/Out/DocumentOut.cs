using Model.VM;

namespace Model.Out
{
    public class DocumentOut : ResultServiceVM
    {
        public DocumentOut()
        {
            this.result = new DocumentVM();
        }

        public DocumentVM result { get; set; }
    }
}

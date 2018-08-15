using Model.VM;

namespace Model.Out
{
    public class DocumentDetailOut : ResultServiceVM
    {
        public DocumentDetailOut()
        {
            this.result = new DocumentDetailVM();
        }

        public DocumentDetailVM result { get; set; }
    }
}

using Model.VM;

namespace Model.Out
{
    public class DocumentDetailDocumentIdOut : ResultServiceVM
    {
        public DocumentDetailDocumentIdOut()
        {
            this.result = new DocumentDetailDocumentIdVM();
        }

        public DocumentDetailDocumentIdVM result { get; set; }
    }
}

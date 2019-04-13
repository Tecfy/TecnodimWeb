using Model.VM;

namespace Model.Out
{
    public class DocumentDetailDocumentIdOut : ResultServiceVM
    {
        public DocumentDetailDocumentIdOut()
        {
            result = new DocumentDetailDocumentIdVM();
        }

        public DocumentDetailDocumentIdVM result { get; set; }
    }
}

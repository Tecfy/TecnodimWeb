using Model.VM;

namespace Model.Out
{
    public class ECMDocumentOut : ResultServiceVM
    {
        public ECMDocumentOut()
        {
            result = new DocumentVM();
        }

        public DocumentVM result { get; set; }
    }
}

using Model.VM;

namespace Model.Out
{
    public class ECMDocumentDetailSaveOut : ResultServiceVM
    {
        public ECMDocumentDetailSaveOut()
        {
            result = new ECMDocumentDetailSaveVM();
        }

        public ECMDocumentDetailSaveVM result { get; set; }
    }
}

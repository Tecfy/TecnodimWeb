using Model.VM;

namespace Model.Out
{
    public class ECMDocumentDetailSaveOut : ResultServiceVM
    {
        public ECMDocumentDetailSaveOut()
        {
            this.result = new ECMDocumentDetailSaveVM();
        }

        public ECMDocumentDetailSaveVM result { get; set; }
    }
}

using Model.VM;

namespace Model.Out
{
    public class DocumentDetailJobIdOut : ResultServiceVM
    {
        public DocumentDetailJobIdOut()
        {
            result = new DocumentDetailJobIdVM();
        }

        public DocumentDetailJobIdVM result { get; set; }
    }
}

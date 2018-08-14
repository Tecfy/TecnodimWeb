using Model.VM;

namespace Model.Out
{
    public class ClippingPageOut : ResultServiceVM
    {
        public ClippingPageOut()
        {
            this.result = new ClippingPageVM();
        }

        public ClippingPageVM result { get; set; }
    }
}

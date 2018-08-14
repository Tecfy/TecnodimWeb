using Model.VM;

namespace Model.Out
{
    public class ClippingOut : ResultServiceVM
    {
        public ClippingOut()
        {
            this.result = new ClippingVM();
        }

        public ClippingVM result { get; set; }
    }
}

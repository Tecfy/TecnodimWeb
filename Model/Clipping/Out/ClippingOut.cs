using Model.VM;

namespace Model.Out
{
    public class ClippingOut : resultServiceVM
    {
        public ClippingOut()
        {
            this.result = new ClippingVM();
        }

        public ClippingVM result { get; set; }
    }
}

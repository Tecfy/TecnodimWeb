using Model.Common;
using Model.VM;

namespace Model.Out
{
    public class ImageOut : resultServiceVM
    {
        public ImageOut()
        {
            this.result = new ImageVM();
        }

        public ImageVM result { get; set; }
    }
}

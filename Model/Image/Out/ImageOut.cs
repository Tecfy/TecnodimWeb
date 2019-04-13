using Model.VM;

namespace Model.Out
{
    public class ImageOut : ResultServiceVM
    {
        public ImageOut()
        {
            result = new ImageVM();
        }

        public ImageVM result { get; set; }
    }
}

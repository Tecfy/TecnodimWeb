using Model.VM;

namespace Model.Out
{
    public class SliceOut : ResultServiceVM
    {
        public SliceOut()
        {
            this.result = new SliceVM();
        }

        public SliceVM result { get; set; }
    }
}

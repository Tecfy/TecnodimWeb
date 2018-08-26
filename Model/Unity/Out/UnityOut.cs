using Model.VM;

namespace Model.Out
{
    public class UnityOut : ResultServiceVM
    {
        public UnityOut()
        {
            this.result = new UnityVM();
        }

        public UnityVM result { get; set; }
    }
}

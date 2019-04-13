using Model.VM;

namespace Model.Out
{
    public class UnityOut : ResultServiceVM
    {
        public UnityOut()
        {
            result = new UnityVM();
        }

        public UnityVM result { get; set; }
    }
}

using Model.VM;

namespace Model.Out
{
    public class UnityEditOut : ResultServiceVM
    {
        public UnityEditOut()
        {
            result = new UnityEditVM();
        }

        public UnityEditVM result { get; set; }
    }
}

using Model.VM;

namespace Model.Out
{
    public class AccessResultOut : ResultServiceVM
    {
        public AccessResultOut()
        {
            this.result = new AccessResultVM();
        }

        public AccessResultVM result { get; set; }
    }
}

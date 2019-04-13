using Model.VM;

namespace Model.Out
{
    public class UserUnityOut : ResultServiceVM
    {
        public UserUnityOut()
        {
            result = new UserUnityVM();
        }

        public UserUnityVM result { get; set; }
    }
}

using Model.VM;

namespace Model.Out
{
    public class UserOut : ResultServiceVM
    {
        public UserOut()
        {
            result = new UserVM();
        }

        public UserVM result { get; set; }
    }
}

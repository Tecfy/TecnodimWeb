using Model.VM;

namespace Model.Out
{
    public class UserOut : ResultServiceVM
    {
        public UserOut()
        {
            this.result = new UserVM();
        }

        public UserVM result { get; set; }
    }
}
